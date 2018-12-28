export interface Error {
	code: string;
	meta: Record<string, any>;
	reasons: Error[];
}
