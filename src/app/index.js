import * as Methods from './methods';
import * as Services from '../services';

export default class App {
	halo4Client: Services.Halo4Client;
	xblClient: Services.XboxLiveClient;

	constructor(h4Client: Services.Halo4Client, xblClient: Services.XboxLiveClient) {
		this.halo4Client = h4Client;
		this.xblClient = xblClient;
	}
}

Object.assign(App.prototype, Methods);
