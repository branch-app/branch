import React from 'react';
import ReactDOM from 'react-dom';
import { createBrowserHistory } from 'history';
import { configure as configureStore } from './store';
import Main from './Main';

const history = createBrowserHistory();
const store = configureStore(history);

ReactDOM.render(<Main history={history} store={store} />, document.getElementById('root'));
