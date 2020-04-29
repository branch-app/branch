import { useScrollYPosition } from 'react-use-scroll-position';
import { Link, NavLink } from 'react-router-dom';
import styled, { css } from 'styled-components';
import {
	Collapse,
	Container,
	DropdownItem,
	DropdownMenu,
	DropdownToggle,
	Nav,
	Navbar,
	NavbarBrand,
	NavbarToggler,
	NavItem,
	UncontrolledDropdown,
} from 'reactstrap';
import React from 'react';

type Mode = 'primary' | 'secondary';

const Navvy: React.FunctionComponent = () => {
	const scrollY = useScrollYPosition();
	const mode = scrollY < 100 ? 'primary' : 'secondary';

	return (
		<StyledNavbar
			fixed={'top'}
			expand={'md'}
			mode={mode}
		>
			<Container>
				<NavbarHeader mode={mode}>
					<Brand
						as={NavLink}
						to={'/'}
						mode={mode}
					>
						{'Branch'}
					</Brand>
					<NavbarToggler />
				</NavbarHeader>
				<Collapse isOpen navbar>
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
									<Link to={'/games/halo-mcc'}>{'Halo 3'}</Link>
								</DropdownItem>
								<DropdownItem divider />
								<DropdownItem>
									<Link to={'/games/halo-infinite'}>{'Halo Infinite'}</Link>
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
			</Container>
		</StyledNavbar>
	);
}

const StyledNavbar = styled(Navbar) <{ mode: Mode }>`
	height: 80px;
	padding-top: 20px;
	padding-left: 80px;
	padding-right: 80px;

	background: transparent;
	border: 0;
	border-radius: 0;

	transition: 0.5s;

	${props => {
		if (props.mode === 'primary') {
			return css`
				.navbar-nav > li {
					> a {
						color: ${props => props.theme.textInverse};

						&, &:hover {
							/* color: ${props => props.theme.textInverse}; */
							opacity: .7;
						}
						&:active, &:focus {
							color: #eee;
						}
					}

					&.open > a {
						&, &:hover {
							background-color: transparent;
							color: white;
							opacity: 0.7;
						}
						&:active, &:focus {
							background-color: transparent;
							color: #eee;;
						}
					}
				}
			`;
		}

		if (props.mode === 'secondary') {
			return css`
				padding-top: 10px;
				height: 70px;

				background: white;
				border-bottom: 1px solid ${props => props.theme.brandPrimary};
			`;
		}
	}}
`;

const NavbarHeader = styled.div<{ mode: Mode }>`
	background-position: left center;
	background-repeat: no-repeat;
	background-size: contain;
	height: 35px;
	padding-left: 45px;

	${({ mode, theme }) => mode === 'primary' && css`
		background-image: url(${theme.images.logo});
	`}
	${({ mode, theme }) => mode === 'secondary' && css`
		background-image: url(${theme.images.logoInverse});
	`}
`;

const Brand = styled(NavbarBrand)<{ mode: Mode }>`
	color: white;
	font-size: 1.25rem;
	line-height: 35px;
	font-weight: 300;

	${({ mode, theme }) => mode === 'primary' && css`
		
	`}
	${({ mode, theme }) => mode === 'secondary' && css`
		color: ${theme.brandPrimary};
		font-weight: 400;
	`}
`;

export default Navvy;
