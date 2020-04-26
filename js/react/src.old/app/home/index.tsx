import { ApplicationState } from './store';
import configureStore from './configureStore';
import { createBrowserHistory } from 'history';
import Main from './main';
import React from 'react';
import ReactDOM from 'react-dom';
import { Store, AnyAction } from 'redux';

declare global {
	interface Window {
		branch: any;
		store: Store<ApplicationState, AnyAction>,
	}
}

const history = createBrowserHistory();
const store = configureStore(history);

window.document.title = 'Branch';
ReactDOM.render(<Main history={history} store={store} />, document.getElementById('root'));
