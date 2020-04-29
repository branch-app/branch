import React from 'react';

const AppContainer: React.FunctionComponent = ({ children }) => {
	return (
		<React.Fragment>
			<header></header>
			<main>{children}</main>
			<header></header>
		</React.Fragment>
	);
};

export default AppContainer;
