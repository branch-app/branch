import { combineReducers, Action, AnyAction, Dispatch } from 'redux';
import { connectRouter, RouterState } from 'connected-react-router';
import { History } from 'history';
import { all, fork } from 'redux-saga/effects';

import { IdentityState } from '../../../lib/platform/identity/store';
import identityReducer from '../../../lib/platform/identity/store/reducers';
import identitySaga from '../../../lib/platform/identity/store/sagas';

export interface ApplicationState {
	router?: RouterState,

	identity: IdentityState,
};

export const createRootReducer = (history: History) => combineReducers<ApplicationState>({
	router: connectRouter(history),

	identity: identityReducer,
});

export function* rootSaga() {
	yield all([
		fork(identitySaga),
	]);
}

export interface ConnectedReduxProps<A extends Action = AnyAction> {
	dispatch: Dispatch<A>
}
