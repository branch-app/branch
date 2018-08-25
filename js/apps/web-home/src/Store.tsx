import { composeWithDevTools } from 'redux-devtools-extension';
import createSagaMiddleware from 'redux-saga';
import rootReducer from './Reducers';
import rootSaga from './Sagas';
import { applyMiddleware, createStore } from 'redux';

export default () => {
	const sagaMiddleware = createSagaMiddleware();
	const store = createStore(rootReducer, composeWithDevTools(applyMiddleware(sagaMiddleware)));

	sagaMiddleware.run(rootSaga);

	return store;
}
