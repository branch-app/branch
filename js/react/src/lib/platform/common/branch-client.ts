import crpc, { Client } from 'crpc';
import CacheClient from './cache-client';
import { Response } from './types';

export default class BranchClient {
	public readonly client: Client;
	public readonly cacheClient: CacheClient;

	constructor(baseUrl: string, key: string) {
		const options = {
			headers: {
				authorization: `bearer ${key}`,
			},
		};

		this.client = crpc(baseUrl, options);
		this.cacheClient = new CacheClient();
	}

	public async get<TRes>(path: string, body: any): Promise<TRes> {
		return await this.client<TRes>(path, body);
	}

	public async getWithCache<TRes extends Response>(cacheKey: string, path: string, body: any): Promise<TRes> {
		const cachedContent = this.cacheClient.readValue<TRes>(cacheKey);

		if (cachedContent)
			return cachedContent;

		const response = await this.get<TRes>(path, body);

		this.cacheClient.writeValue(cacheKey, response);

		return response;
	}
}
