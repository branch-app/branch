import App from '../index';
import { GameMode } from '../../services/halo4-client';
import { IdentityType } from '../../services';

export default class Halo4 {
	_app: App;
	_serviceRecordScopes: string[] = ['achievements', 'difficulty', 'spartanops'];

	constructor(app: App) {
		this._app = app;
	}

	async getServiceRecord(gamertag: string) {
		const [
			identity,
			serviceRecord,
			headerMatches,
			metadata,
			metadataOptions,
		] = await Promise.all([
			this._app._xblClient.getIdentity(gamertag, IdentityType.Gamertag),
			this._app._halo4Client.getServiceRecord(gamertag, IdentityType.Gamertag),
			this._app._halo4Client.getRecentMatches(gamertag, IdentityType.Gamertag, GameMode.WarGames, 0, 5),
			this._app._halo4Client.getMetadata(...this._serviceRecordScopes),
			this._app._halo4Client.getMetadataOptions(),
		]);

		return {
			identity,
			serviceRecord,
			headerMatches,
			metadata,
			metadataOptions,
		};
	}
}
