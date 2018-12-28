import { Store, createStore, applyMiddleware } from 'redux';
import createSagaMiddleware from 'redux-saga';
import { routerMiddleware } from 'connected-react-router';
import { composeWithDevTools } from 'redux-devtools-extension';
import { History } from 'history';
import { ApplicationState, createRootReducer, rootSaga } from './store';

export default function configureStore(history: History) : Store<ApplicationState> {
	const composeEnhancers = composeWithDevTools({});
	const sagaMiddleware = createSagaMiddleware({
		context: { },
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
