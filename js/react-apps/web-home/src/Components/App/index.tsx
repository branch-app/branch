import Footer from '../../Atoms/Footer';
import Navbar from '../../Atoms/Navbar';
import * as React from 'react';
import './index.css';

export interface IProps {
	children: any;
};

export default ({ children }: IProps) => (
	<div className={'component-app'}>
		<Navbar />
		<main>
			{children}
		</main>
		<Footer />
	</div>
);
