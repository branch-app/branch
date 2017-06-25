import * as Methods from './methods';

export default class App {
	constructor(config, db) {
		this.config = config;
		this.db = db;
	}
}

Object.assign(App.prototype, Methods);
