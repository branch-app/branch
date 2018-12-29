import { Store, createStore, applyMiddleware } from 'redux';
import createSagaMiddleware from 'redux-saga';
import { routerMiddleware } from 'connected-react-router';
import { composeWithDevTools } from 'redux-devtools-extension';
import { History } from 'history';
import { ApplicationState, createRootReducer, rootSaga } from './store';

import IdentityClient from '../../lib/platform/identity/client';
import CacheClient from '../../lib/platform/common/cache-client';

export default function configureStore(history: History) : Store<ApplicationState> {
	const composeEnhancers = composeWithDevTools({});
	const sagaMiddleware = createSagaMiddleware({
		context: { ...createClients() },
	});

	const initialState: ApplicationState = {
		identity: {}
	};

	const store = createStore(
		createRootReducer(history),
		initialState,
		composeEnhancers(applyMiddleware(sagaMiddleware, routerMiddleware(history))),
	);

	sagaMiddleware.run(rootSaga);

	return (window.store = store);
}

function createClients() {
	// All clients share the same cache client
	const cacheClient = new CacheClient();

	return {
		identityClient: new IdentityClient(
			'https://service-identity.branchapp.co',
			'imNAGKE74LaVtgioydX64GBrEn3x7ZCR2KSw8qA4LZy3UaDc3AltWoxAPQH5QSqe',
			cacheClient,
		),
	};
}
