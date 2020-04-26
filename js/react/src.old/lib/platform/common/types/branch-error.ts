export interface BranchError {
	code: string;
	meta: Record<string, any>;
	reasons: BranchError[];
}
