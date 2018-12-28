import { IdentityState } from '../';
import { Reducer, AnyAction } from 'redux';

export const initialState: IdentityState = {};

const reducer: Reducer<IdentityState> = (state = initialState, { type, payload }: AnyAction) => {
	return state;
}

export default reducer;
