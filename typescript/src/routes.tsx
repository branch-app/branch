import { BrowserRouter, Route, Switch } from 'react-router-dom';
import React from 'react';
import Home from './containers/Home';
import AppContainer from './containers/AppContainer';

const Routes: React.FunctionComponent = () => {
	return (
		<React.Fragment>
			<BrowserRouter>
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
