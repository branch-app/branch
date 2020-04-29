import crpc, { Client } from "crpc";

interface Options {
	services: {
		name: string;
		baseUrl: string;
		key: string;
	}[];
}

export default class BranchClient {
	private readonly _options: Options;
	private readonly _clients: Record<string, Client> = {};

	constructor(options: Options) {
		this._options = options;

		this._options.services.forEach(s => {
			this._clients[s.name] = crpc(s.baseUrl, { headers: { authorization: `bearer ${s.key}` } });
		});
	}

	async do<T>(service: string, endpoint: string, version: string, body: Record<string, unknown>) {
		const svc = this._clients[service];

		return await svc<T>(`${version}/${endpoint}`, body);
	}
}
