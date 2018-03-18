import { Collection as MCollection } from 'mongodb';
import { MongoNativeId } from './id-providers';
import log from '@branch-app/log';

export default class Collection<T> {
	_collection: MCollection;

	// usually overridden by subclasses
	static idProvider = MongoNativeId;
	async setupIndexes(): Promise<void> {}
	mapInput(input: T): {} { return input; }
	mapOutput(output: {}): T { return output; }

	constructor(collection: MCollection) {
		this._collection = collection;
	}

	async index(keys: string[]|{}[]|{}, options: ?{}): Promise<void> {
		if (!Array.isArray(keys)) {
			await this._collection.createIndex(keys, options);

			return;
		}

		const specs = keys.map(k => {
			if (typeof k === 'object')
				return k;
			const spec = {};

			spec[k] = 1;

			return spec;
		});

		await Promise.all(specs.map(async s => {
			await this._collection.createIndex(s, options);
		}));
	}

	// create one document
	// returns new document
	async createOne(input: T): Promise<T> {
		const mappedInput = mapObjIn(input, true, this);
		const result = await this._collection.insertOne(mappedInput);

		return mapObjOut(result.ops[0], this);
	}

	// creates many documents
	// returns new documents
	async createMany(input: T[]): Promise<T[]> {
		const mappedInput = input.map(i => mapObjIn(i, true, this));
		const result = await this._collection.insertMany(mappedInput);

		return result.ops.map(o => mapObjOut(o, this));
	}

	// find one document by filter or id
	// returns found document
	// input not mapped because Mongo operators are used
	async findOne(filter: {} | string, options: {}): Promise<T> {
		if (typeof filter === 'string')
			filter = { _id: parseIdForFind(filter, this) };

		const object = await this._collection.find(filter, options).limit(1).next();

		if (!object)
			throw log.debug('not_found');

		return mapObjOut(object, this);
	}

	// find many documents by filter or ids
	// returns array of found documents
	// input not mapped because Mongo operators are used
	async findMany(filter: {}|string[]): Promise<T[]> {
		if (Array.isArray(filter))
			filter = { _id: { $in: filter.map(i => parseIdForFind(i, this)) } };
		const results = await this._collection.find(filter).toArray();

		return results.map(o => mapObjOut(o, this));
	}

	// count documents by filter or ids
	// returns number of matching documents
	// input not mapped because Mongo operators are used
	async count(filter: {}|string[]): Promise<number> {
		if (Array.isArray(filter))
			filter = { _id: { $in: filter.map(i => parseIdForFind(i, this)) } };

		return await this._collection.count(filter);
	}

	// update one document by id
	// returns updated document
	// input not mapped because Mongo operators are used
	async updateOne(id: string, update: {}): Promise<T> {
		const filter = { _id: parseIdForFind(id, this) };
		const options = { returnOriginal: false };
		const result = await this._collection.findOneAndUpdate(filter, update, options);

		if (!result.ok || !result.value)
			throw log.warn('not_found', { result });

		return mapObjOut(result.value, this);
	}

	// update many documents by filter or id
	// returns number of documents updated
	// input not mapped because Mongo operators are used
	async updateMany(filter: {}|string[], update: {}): Promise<number> {
		if (Array.isArray(filter))
			filter = { _id: { $in: filter.map(i => parseIdForFind(i, this)) } };

		const result = await this._collection.updateMany(filter, update);

		return result.matchedCount;
	}

	// create or update one document by filter
	// returns new or updated document
	// input not mapped because Mongo operators are used
	async upsertOne(filter: {}, update: {}): Promise<T> {
		const options = { upsert: true, returnOriginal: false };
		const result = await this._collection.findOneAndUpdate(filter, update, options);

		if (!result.ok || !result.value)
			throw log.warn('not_found', { result });

		return mapObjOut(result.value, this);
	}

	// todo: is upsertMany possible?

	// delete one document by id
	async deleteOne(id: string): Promise<void> {
		const mappedId = parseIdForFind(id, this);
		const result = await this._collection.deleteOne({ _id: mappedId });

		if (!result.deletedCount)
			throw log.info('not_found', { result });
	}

	// deletes many documents by filter or id
	// returns number of documents deleted
	// input not mapped because Mongo operators are used
	async deleteMany(filter: {}|string[]): Promise<number> {
		if (Array.isArray(filter))
			filter = { _id: { $in: filter.map(i => parseIdForFind(i, this)) } };

		const result = await this._collection.deleteMany(filter);

		return result.deletedCount;
	}
}

function parseIdForFind<T>(input: string, obj: Collection<T>): {} {
	const idp = obj.constructor.idProvider;

	try {
		return idp.parse(input);
	} catch (error) {
		if (error.code === 'invalid_id')
			throw log.info('not_found');
		throw error;
	}
}

// maps input from the application to the MongoDB fields
function mapObjIn<T>(input: T, idNeeded: boolean, obj: Collection<T>): {} {
	if (input._id)
		throw log.error('id_rules_violated', { input });

	// shallow copy
	input = { ...input };

	const idp = obj.constructor.idProvider;

	if (input.id) {
		input._id = idp.parse(input.id);
		input.id = void 0;
	}

	if (!input._id && idNeeded)
		input._id = idp.generate();

	return obj.mapInput(input);
}

// maps MongoDB output to the application-friendly format
function mapObjOut<T>(output: {}, obj: Collection<T>): T {
	if (output.id)
		throw log.error('id_rules_violated', { output });

	// not sure why this would happen
	if (!output._id)
		return output;

	// shallow copy
	output = { ...output };

	const idp = obj.constructor.idProvider;

	output.id = idp.stringify(output._id);
	output._id = void 0;

	return obj.mapOutput(output);
}
