import Root from './components/layout/Root';
import { createGlobalStyle } from 'styled-components';
import * as React from 'react';
import { Route, Switch } from 'react-router-dom';

import HomePage from './pages/Home';

const GlobalStyle = createGlobalStyle`
	html, body {
		margin: 0;
		padding: 0;

		font-family: 'Open Sans', -apple-system, system-ui, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
		background-color: #3d3d7c !important;
	}
`;

const Routes: React.FunctionComponent = () => {
	return (
		<Root>
			<GlobalStyle />
			<Switch>
				<Route exact path={'/'} component={HomePage} />
				<Route component={() => <div>{'Not Found'}</div>} />
			</Switch>
		</Root>
	);
}

export default Routes;
