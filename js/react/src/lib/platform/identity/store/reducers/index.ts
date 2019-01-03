import { Reducer, AnyAction } from 'redux';
import {
	IdentityActionTypes,
	IdentityState,
	initialIdentityState as initialState,
} from '..';

const reducer: Reducer<IdentityState> = (state = initialState, { type, payload }: AnyAction) => {
	switch (type) {
		case IdentityActionTypes.FETCH_IDENTITY: {

		}
	}

	return state;
}

export default reducer;
