import { Container } from 'reactstrap';
import Navvy from '../components/organisms/Navvy';
import React from 'react';
import styled from 'styled-components';

const AppContainer: React.FunctionComponent = ({ children }) => {
	return (
		<React.Fragment>
			<Navvy />
			<main>
				<Container>
					{children}
				</Container>
			</main>
			<footer></footer>
		</React.Fragment>
	);
};

export default AppContainer;
