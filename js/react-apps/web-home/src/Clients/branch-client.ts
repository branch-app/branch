import crpc, { Client } from 'crpc';

export default class BranchClient {
	private client: Client;

	constructor(baseUrl: string, key: string) {
		const options = {
			headers: {
				authorization: `bearer ${key}`,
			},
		};

		this.client = crpc(baseUrl, options);
	}

	public async execute<TReq, TRes>(path: string, body: TReq): Promise<TRes> {
		return await this.client<TReq, TRes>(path, body);
	}
}
