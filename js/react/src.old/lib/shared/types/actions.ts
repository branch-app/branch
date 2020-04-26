import { BranchError } from '../../platform/common/types';

export interface AsyncState<T> {
	fetching: boolean;
	result: T | null;
	error: BranchError | null;
}
