import log from '@branch-app/log';
import * as Collections from './collections';
import { Db, MongoClient } from 'mongodb';

type Config = {
	uri: string;
	sslCA?: string[];
};

export default class Database {
	db: Db;
	collections: {}[];

	static async connect(config: Config): Promise<Database> {
		// for this particular service, majority is worth having
		// if copying, evaluate your write concern needs properly
		const opts = {
			w: 'majority',
		};

		if (config.sslCA) {
			opts.sslValidate = true;
			opts.sslCA = config.sslCA;
		}

		const db = await MongoClient.connect(config.uri, opts);

		log.info('database_connected');

		return new Database(db);
	}

	constructor(db) {
		this.db = db;
		this.collections = [];

		for (const [key, value] of Object.entries(Collections)) {
			const mongoCollection = db.collection(key);
			const instance = new value(mongoCollection);

			this[key] = instance;
			this.collections.push(instance);
		}

		// not awaited
		setupIndexes(this.collections);
	}
}

async function setupIndexes(collections): Promise<void> {
	try {
		await Promise.all(collections.map(c => c.setupIndexes()));
		log.info('database_indexes_setup');
	} catch (error) {
		log.error('index_creation_failed', [error]);
	}
}
