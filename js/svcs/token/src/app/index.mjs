import * as Methods from './methods';

export default class App {
	config;
	chromieTalkie;
	redis;

	constructor(config, chromieTalkie, redis) {
		this.config = config;
		this.chromieTalkie = chromieTalkie;
		this.redis = redis;
	}
}

Object.assign(App.prototype, Methods);
