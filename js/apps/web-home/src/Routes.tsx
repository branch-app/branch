import App from './Components/App';
// import ErrorPage from './Components/ErrorPage';
import Home from './Components/Home';
import * as React from 'react';
import { Route, BrowserRouter as Router, Switch } from 'react-router-dom';

export default (): React.ReactElement<Router> => (
	<Router>
		<App>
			<Switch>
				<Route
					component={Home}
					exact={true}
					path={'/'}
				/>
			</Switch>
		</App>
	</Router>
);

{/*
<Route component={
	() => (
		<ErrorPage
			description={'The page you are looking for doesn\'t exist.. Are you sure it ever even existed? 🤔'}
			title={'Not found'}
		/>
	)}
/>
*/}
