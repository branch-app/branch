import jsonClient from 'json-client';

export default class XboxLiveClient {
	constructor(baseUrl, key) {
		this._client = jsonClient(baseUrl, {
			headers: {
				authorization: `bearer ${key}`,
			},
		});
	}

	async getIdentity(ident: string, type: string) {
		return await this._client('post', '/1/2017-05-21/get_identity', null, {
			type,
			value: ident,
		});
	}
}
