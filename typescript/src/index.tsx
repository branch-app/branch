/* eslint-disable sort-imports */
import { Provider } from 'react-redux';
import React from 'react';
import ReactDOM from 'react-dom';
import Routes from './routes';
import { ThemeProvider } from 'styled-components';
import configureStore from './store';
import { createBrowserHistory } from 'history';
import theme from './theme';
import 'bootstrap/dist/css/bootstrap.min.css';

const history = createBrowserHistory();
const store = configureStore(history);

const AppProvider = (
	<Provider store={store}>
		<ThemeProvider theme={theme}>
			<Routes />
		</ThemeProvider>
	</Provider>
);

// Render to the DOM
ReactDOM.render(AppProvider, window.document.getElementById('branch-goes-here-xoxo'));
