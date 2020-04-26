import { History } from 'history';
import createSagaMiddleware from "@redux-saga/core";
import crpc from "crpc";
import { all } from 'redux-saga/effects';
import { routerMiddleware, connectRouter } from 'connected-react-router';
import { composeWithDevTools } from 'redux-devtools-extension';
import { applyMiddleware, createStore, combineReducers } from 'redux';

export interface ApplicationState {

}

export function configure(history: History<any>) {
	const composeEnhancers = composeWithDevTools({});
	const sagaMiddleware = createSagaMiddleware();
	const initialState: ApplicationState = getInitialState();
	const store = createStore(
		createRootReducer(history),
		initialState,
		composeEnhancers(applyMiddleware(sagaMiddleware, routerMiddleware(history))),
	);
	const context = {
		clients: {
			identity: createClient('identity', 'imNAGKE74LaVtgioydX64GBrEn3x7ZCR2KSw8qA4LZy3UaDc3AltWoxAPQH5QSqe'),
		},
	};

	sagaMiddleware.setContext(context);
	sagaMiddleware.run(rootSaga);

	return store;
}

function createRootReducer(history: History) {
	return combineReducers<ApplicationState>({
		router: connectRouter(history),

		internal: combineReducers({
			
		}),
	});
}

function* rootSaga() {
	yield all([
		
	]);
}

function getInitialState(): ApplicationState {
	return {

	};
}

function createClient(name: string, key: string) {
	return crpc(
		`https://service-${name}.branchapp.co/1`,
		{
			headers: {
				authorization: `bearer ${key}`,
			},
		},
	);
}
