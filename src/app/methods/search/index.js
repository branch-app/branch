import App from '../../index';
import { IdentityType } from '../../../services';

export default class Halo4 {
	_app: App;

	constructor(app: App) {
		this._app = app;
	}

	async showSearchResults(gamertag: string) {
		const [
			identity,
			h4PlayerCard,
		] = await Promise.all([
			this._app._xblClient.getIdentity(gamertag, IdentityType.Gamertag),
			this._app._halo4Client.getPlayerCard(gamertag, IdentityType.Gamertag),
		]);

		return {
			identity,
			h4PlayerCard,
		};
	}
}
