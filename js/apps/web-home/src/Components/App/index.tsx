import Navbar from '../../Atoms/Navbar';
import * as React from 'react';
import { Container } from 'reactstrap';
import './index.css';

export interface IProps {
	children: any;
};

export default ({ children }: IProps) => (
	<div>
		<Navbar />
		<Container>
			{children}
		</Container>
	</div>
);
