import { NavLink } from 'react-router-dom';
import {
	Collapse,
	UncontrolledDropdown,
	DropdownItem,
	DropdownToggle,
	DropdownMenu,
	Navbar as RSNavbar,
	NavbarBrand,
	NavbarToggler,
	Nav,
	NavItem,
} from 'reactstrap';
import * as React from 'react'
import './index.css';

interface IState {
	isOpen: boolean;
}

export default class Navbar extends React.PureComponent<{}, IState> {
	public constructor(props: {}) {
		super(props);

		this.state = {
			isOpen: false,
		};
	}

	public toggle = () => this.setState(prevState => ({ isOpen: !prevState.isOpen }));

	public render() {
		const { isOpen } = this.state;

		return (
			<RSNavbar
				className={'component-branch-navbar'}
				color={'light'}
				light
				expand={'md'}
			>
				<NavbarBrand href={'/'}>{'🌳 Branch'}</NavbarBrand>
				<NavbarToggler onClick={this.toggle} />
				<Collapse
					isOpen={isOpen}
					navbar
				>
					<Nav
						className={'ml-auto'}
						navbar
					>
						<NavItem>
							<NavLink
								className={'nav-link'}
								to={'/components/'}
							>
								{'Components'}
							</NavLink>
						</NavItem>
						<NavItem>
							<NavLink
								className={'nav-link'}
								to={'https://github.com/reactstrap/reactstrap'}
							>
								{'GitHub'}
							</NavLink>
						</NavItem>
						<UncontrolledDropdown
							nav
							inNavbar
						>
							<DropdownToggle
								nav
								caret
							>
								{'Options'}
							</DropdownToggle>
							<DropdownMenu right>
								<DropdownItem>
									{'Option 1'}
								</DropdownItem>
								<DropdownItem>
									{'Option 2'}
								</DropdownItem>
								<DropdownItem divider />
								<DropdownItem>
									{'Reset'}
								</DropdownItem>
							</DropdownMenu>
						</UncontrolledDropdown>
					</Nav>
				</Collapse>
			</RSNavbar>
		);
	}
}
