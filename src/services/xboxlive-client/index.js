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
		return await this._client('get', `/v1/identity/${type}(${ident})`);
	}
}
