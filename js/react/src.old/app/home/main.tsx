import * as React from 'react';
import { Store } from 'redux'
import { History } from 'history'
import { ApplicationState } from './store';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'connected-react-router';
import Routes from './routes';

export interface IProps {
	store: Store<ApplicationState>;
	history: History;
}

const Main: React.FunctionComponent<IProps> = ({ store, history }) => (
	<Provider store={store}>
		<ConnectedRouter history={history}>
			<Routes />
		</ConnectedRouter>
	</Provider>
);

export default Main;
