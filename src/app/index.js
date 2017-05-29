import * as Methods from './methods';
import * as Services from '../services';

export default class App {
	_halo4Client: Services.Halo4Client;
	_xblClient: Services.XboxLiveClient;

	Halo4: Methods.Halo4;

	constructor(h4Client: Services.Halo4Client, xblClient: Services.XboxLiveClient) {
		this._halo4Client = h4Client;
		this._xblClient = xblClient;

		this.Halo4 = new Methods.Halo4(this);
	}
}

