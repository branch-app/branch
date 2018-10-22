import App from './Components/App';
import Halo4ServiceRecord from './Components/Halo4/ServiceRecord';
import { Provider } from 'react-redux';
import Home from './Components/Home';
import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import { ConnectedRouter } from 'connected-react-router';
import { Store } from 'redux';
import { History } from 'history';
import ScrollToTop from './Atoms/ScrollToTop';

export default (store: Store, history: History) => (
	<Provider store={store}>
		<ConnectedRouter history={history}>
			<App>
				<ScrollToTop />
				<Switch>
					<Route
						component={Home}
						exact={true}
						path={'/'}
					/>
					<Route
						component={Halo4ServiceRecord}
						path={'/halo-4/:gamertag'}
					/>

					<Route component={() => (<span>{'fak, nothing here'}</span>)} />
				</Switch>
			</App>
		</ConnectedRouter>
	</Provider>
);
