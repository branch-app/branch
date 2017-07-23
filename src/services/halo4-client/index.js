import jsonClient from 'json-client';

export const GameMode = {
	WarGames: 3,
	Campaign: 4,
	SpartanOps: 5,
	CustomGames: 6,
};

export default class Halo4Client {
	constructor(baseUrl, key) {
		this._client = jsonClient(baseUrl, {
			headers: {
				authorization: `bearer ${key}`,
			},
		});
	}

	async getMetadata(...types: string[]) {
		return await this._client('post', '/1/2017-05-21/get_metadata', null, {
			types,
		});
	}

	async getMetadataOptions() {
		return await this._client('post', '/1/2017-05-21/get_options');
	}

	async getPlayerCard(ident: string, type: string) {
		return await this._client('post', '/1/2017-07-22/get_playercard', null, {
			type,
			value: ident,
		});
	}

	async getRecentMatches(ident: string, type: string, mode: number, startAt: number = 0, count: number = 25) {
		return await this._client('post', '/1/2017-05-29/get_recent_matches', null, {
			identity: {
				type,
				value: ident,
			},
			gameModeId: mode,
			startAt,
			count,
		});
	}

	async getServiceRecord(ident: string, type: string) {
		return await this._client('post', '/1/2017-05-21/get_service_record', null, {
			type,
			value: ident,
		});
	}
}
