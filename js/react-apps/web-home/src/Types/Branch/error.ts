namespace Branch {
	export interface Error {
		code: string;
		meta: { [x: string]: any },
		reasons: Error[];
	}
}
