import { BrowserRouter, Route, Switch } from 'react-router-dom';
import React from 'react';
import Home from './containers/Home';
import AppContainer from './containers/AppContainer';
import { createGlobalStyle } from 'styled-components';

const GlobalStyle = createGlobalStyle`
	body {
		background-color: ${props => props.theme.brandPrimary};
	}

	* {
		font-family: 'Open Sans', "Helvetica Neue", Helvetica, Arial, sans-serif;
	}
`;

const Routes: React.FunctionComponent = () => {
	return (
		<React.Fragment>
			<BrowserRouter>
				<GlobalStyle />
				<AppContainer>
					<Switch>
						<Route path={'/'} component={Home} />
					</Switch>
				</AppContainer>
			</BrowserRouter>
		</React.Fragment>
	);
};

export default Routes;
