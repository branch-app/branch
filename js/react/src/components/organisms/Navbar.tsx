import React from 'react';
import { Navbar as BaseNavbar, NavbarProps } from 'reactstrap';
import styled from 'styled-components';

const StyledNavbar = styled(BaseNavbar)<NavbarProps>`

`;

const Navbar: React.FunctionComponent = () => (
	<StyledNavbar
		fixed={'top'}
	>
		
	</StyledNavbar>
);

export default Navbar;
