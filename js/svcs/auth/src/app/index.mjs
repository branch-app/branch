import * as Methods from './methods';

export default class App {
	config;
	redis;

	constructor(config, redis) {
		this.config = config;
		this.redis = redis;
	}
}

Object.assign(App.prototype, Methods);
