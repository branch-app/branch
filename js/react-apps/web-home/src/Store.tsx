import { applyMiddleware, createStore, Store } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import { connectRouter, routerMiddleware } from 'connected-react-router';
import { createBrowserHistory, History } from 'history';
import BranchClient from './Clients/branch-client';
import CacheClient from './Clients/cache-client';
import config from './config.json';
import createSagaMiddleware from 'redux-saga';
import rootReducer from './Reducers';
import rootSaga from './Sagas';

export default (): { store: Store, history: History } => {
	const services = config.services;

	const history = createBrowserHistory();
	const sagaMiddleware = createSagaMiddleware({
		context: {
			cacheClient: new CacheClient(),

			halo4Client: new BranchClient(services.halo4.url, services.halo4.key),
			identityClient: new BranchClient(services.identity.url, services.identity.key),
		},
	});

	const store = createStore(
		connectRouter(history)(rootReducer),
		composeWithDevTools(applyMiddleware(sagaMiddleware, routerMiddleware(history))),
	);

	sagaMiddleware.run(rootSaga);

	return { store, history };
}
