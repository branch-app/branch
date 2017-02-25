import * as Methods from './methods';

export default class App {
	config;
	
	constructor(config) {
		this.config = config;
	}
}

Object.assign(App.prototype, Methods);
