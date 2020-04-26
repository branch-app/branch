import { ApplicationState } from './store';
import { ConnectedRouter } from 'connected-react-router';
import { History } from 'history';
import { Provider } from 'react-redux';
// import Routes from './Routes';
import { Store } from 'redux';
import * as React from 'react';
import { ThemeProvider } from 'styled-components';
import { Switch, Route } from 'react-router';
import Header from './components/templates/Header';
import Footer from './components/templates/Footer';

export interface MainProps {
	history: History;
	store: Store<ApplicationState>;
}

const theme = {

};

const Main: React.FunctionComponent<MainProps> = ({ store, history }) => (
	<Provider store={store}>
		<ConnectedRouter history={history}>
			<ThemeProvider theme={theme}>
				<React.Fragment>
					{/* <GlobalStyle /> */}
					<Header />
					<Switch>
						<Route component={() => <div>{'not found'}</div>} />
					</Switch>
					<Footer />
				</React.Fragment>
			</ThemeProvider>
		</ConnectedRouter>
	</Provider>
);

export default Main;
