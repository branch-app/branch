import RedisClient from '@branch-app/redis-client';
import * as Methods from './methods';

export default class App {
	config: {};
	redis: RedisClient;

	constructor(config: {}, redis: RedisClient) {
		this.config = config;
		this.redis = redis;
	}
}

Object.assign(App.prototype, Methods);
