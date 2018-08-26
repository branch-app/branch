import classnames from 'classnames';
import { NavLink, Link } from 'react-router-dom';
import {
	Collapse,
	UncontrolledDropdown,
	DropdownItem,
	DropdownToggle,
	DropdownMenu,
	Navbar as RSNavbar,
	NavbarToggler,
	Nav,
	NavItem,
} from 'reactstrap';
import * as React from 'react';
import './index.css';

let scrollTicking: boolean = false;

interface IState {
	isPrimaryStyle: boolean;
	isOpen: boolean;
}

export default class Navbar extends React.PureComponent<{}, IState> {
	private navStyleChangeOffset: number = 55;

	public constructor(props: {}) {
		super(props);

		this.state = {
			isOpen: false,
			isPrimaryStyle: window.scrollY < this.navStyleChangeOffset,
		};
	}

	public componentDidMount() {
		window.addEventListener('scroll', this.handleScroll);
	}

	public componentWillUnmount() {
		window.removeEventListener('scroll', this.handleScroll);
	}

	public toggle = () => this.setState(prevState => ({ isOpen: !prevState.isOpen }));

	public handleScroll = () => {
		if (scrollTicking)
			return;

		scrollTicking = true;
		const offset = window.scrollY;

		window.requestAnimationFrame(() => {
			this.setState({ isPrimaryStyle: offset < this.navStyleChangeOffset });

			scrollTicking = false;
		});
	}

	public render() {
		const { isOpen, isPrimaryStyle } = this.state;
		const classes = classnames(
			'component-branch-navbar',
			{ 'primary': isPrimaryStyle },
			{ 'secondary': !isPrimaryStyle },
		);

		return (
			<RSNavbar
				className={classes}
				fixed={'top'}
				expand={'md'}
			>
				<div className={'navbar-header'}>
					<Link
						className={'navbar-brand'}
						to={'/'}
					>
						{'Branch'}
					</Link>

					<NavbarToggler onClick={this.toggle} />
				</div>
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
								to={'/blog'}
							>
								{'Blog'}
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
								{'Games'}
							</DropdownToggle>
							<DropdownMenu right>
								<DropdownItem>
									<Link to={'/games/halo-3'}>{'Halo 3'}</Link>
								</DropdownItem>
								<DropdownItem>
									<Link to={'/games/halo-3-odst'}>{'Halo 3: ODST'}</Link>
								</DropdownItem>
								<DropdownItem>
									<Link to={'/games/halo-reach'}>{'Halo: Reach'}</Link>
								</DropdownItem>
								<DropdownItem>
									<Link to={'/games/halo-4'}>{'Halo 4'}</Link>
								</DropdownItem>
								<DropdownItem>
									<Link to={'/games/halo-5-guardians'}>{'Halo 5: Guardians'}</Link>
								</DropdownItem>
								<DropdownItem>
									<Link to={'/games/halo-wars-2'}>{'Halo Wars 2'}</Link>
								</DropdownItem>
								<DropdownItem divider />
								<DropdownItem>
									<Link to={'/games/destiny'}>{'Destiny'}</Link>
								</DropdownItem>
								<DropdownItem>
									<Link to={'/games/destiny-2'}>{'Destiny 2'}</Link>
								</DropdownItem>
								<DropdownItem divider />
								<DropdownItem>
									<Link to={'/games/titanfall'}>{'Titanfall'}</Link>
								</DropdownItem>
								<DropdownItem>
									<Link to={'/games/titanfall-2'}>{'Titanfall 2'}</Link>
								</DropdownItem>
							</DropdownMenu>
						</UncontrolledDropdown>
						<NavItem>
							<NavLink
								className={'nav-link'}
								to={'/about'}
							>
								{'About'}
							</NavLink>
						</NavItem>
					</Nav>
				</Collapse>
			</RSNavbar>
		);
	}
}
