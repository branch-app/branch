import crpc, { Client } from 'crpc';
import CacheClient from './cache-client';
import { Response } from './types';

export default class BranchClient {
	readonly client: Client;
	readonly cacheClient: CacheClient;

	constructor(baseUrl: string, key: string, cacheClient: CacheClient) {
		const options = {
			headers: {
				authorization: `bearer ${key}`,
			},
		};

		this.client = crpc(baseUrl, options);
		this.cacheClient = cacheClient;
	}

	async get<TRes>(path: string, body: any): Promise<TRes> {
		return await this.client<TRes>(path, body);
	}

	async getWithCache<TRes extends Response>(cacheKey: string, path: string, body: any): Promise<TRes> {
		const cachedContent = await this.cacheClient.get<TRes>(cacheKey);

		if (cachedContent)
			return cachedContent;

		const response = await this.get<TRes>(path, body);

		await this.cacheClient.set(cacheKey, response);

		return response;
	}
}
