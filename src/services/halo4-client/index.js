import jsonClient from 'json-client';

export default class Halo4Client {
	constructor(baseUrl, key) {
		this._client = jsonClient(baseUrl, {
			headers: {
				authorization: `bearer ${key}`,
			},
		});
	}

	async getServiceRecord(ident: string, type: string) {
		return await this._client('get', `/v1/identity/${type}(${ident})/service-record`);
	}

	async getMetadata(...types: string[]) {
		return await this._client('get', '/v1/metadata', {
			type: types.join(),
		});
	}

	async getMetadataOptions() {
		return await this._client('get', '/v1/metadata/options');
	}
}
