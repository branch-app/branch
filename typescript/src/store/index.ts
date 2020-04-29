import { History } from 'history';
import { composeWithDevTools } from 'redux-devtools-extension';
import createSagaMiddleware from 'redux-saga';
import { Dispatch, Store, applyMiddleware, createStore } from 'redux';
import { combineReducers } from 'redux';
import { RouterState, connectRouter } from 'connected-react-router';
import { all, fork } from 'redux-saga/effects';
import BranchClient from '../api/branch-client';

export interface ApplicationState {
	router: RouterState;
}

function configureStore(history: History) {
	const composeEnhancers = composeWithDevTools({});
	const sagaMiddleware = createSagaMiddleware();
	const initialState: Omit<ApplicationState, 'router'> = {};

	const store = createStore(
		createRootReducer(history),
		initialState,
		composeEnhancers(applyMiddleware(sagaMiddleware)),
	);

	const sagaContext = {
		branchClient: createBranchClient(),

		store,
	};

	sagaMiddleware.setContext(sagaContext);
	sagaMiddleware.run(rootSaga);

	return store;
}

function createRootReducer(history: History) {
	return combineReducers<ApplicationState>({
		router: connectRouter(history),
	});
}

function createBranchClient(): BranchClient {
	const services = [{
		name: 'identity',
		baseUrl: 'https://service-identity.branch.golf/1',
		key: 'yYaA2vErSPXdWtqjnzKLqHcQ5g8uzDDJ3UGFN58ZgEaJ4v5dcj',
	}];

	return new BranchClient({ services });
}

function* rootSaga() {
	yield all([]);
}

export default configureStore;
