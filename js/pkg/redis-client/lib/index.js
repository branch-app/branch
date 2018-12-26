const redis = require('redis');
const snakeize = require('snakeize');
const util = require('util');

const defaultTimeout = 3000;

class RedisClient {
	constructor(options) {
		const opts = Object.assign({}, {
			connectTimeout: defaultTimeout,
		}, options);

		this._client = redis.createClient(snakeize(opts));

		if (opts.database)
			this._client.select(opts.database);

		this.get = util.promisify(this._client.get.bind(this._client));
		this.set = util.promisify(this._client.set.bind(this._client));
	}

	get client() {
		return this._client;
	}

	static async connect(options) {
		const client = new RedisClient(options);

		return await new Promise((resolve, reject) => {
			client.client.on('error', reject);

			client.client.on('connect', () => {
				resolve(client);
			});
		});
	}
}

module.exports = RedisClient;
