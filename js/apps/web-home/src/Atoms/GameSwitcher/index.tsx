import { Link } from 'react-router-dom';
import classnames from 'classnames';
import data from './data.json';
import {
	Container,
	Popover,
	PopoverBody,
} from 'reactstrap';
import * as React from 'react';
import './index.css';

export interface IProps {
	identity: string;
	selectedGame: string;
	selectedTitle: string;
}

interface IState {
	popovers: {};
}

export default class GameSwitcher extends React.PureComponent<IProps, IState> {
	constructor(props: IProps) {
		super(props);

		this.state = { popovers: { } };
	}

	public renderDropdown(key: string, target: string) {
		const { identity, selectedTitle } = this.props;
		const { popovers } = this.state;
		const game = data.games[key];

		return (
			<div
				className={'dropdown-selector'}
				key={key}
			>
				<i className={'fas fa-chevron-down'}></i>

				<Popover
					className={'game-switcher-popover'}
					isOpen={popovers[key]}
					placement={'bottom'}
					target={target}
					toggle={() => this.togglePopover(key)}
				>
					<PopoverBody>
						{Object.keys(game.titles).map(k => {
							const title = game.titles[k];
							const active = k === selectedTitle;

							return (
								<Link
									className={classnames('title', { active })}
									key={k}
									to={this.replaceUrl(title.url, identity)}
								>
									<div
										className={'game-icon'}
										style={{ backgroundImage: `url('/images/switcher-bar/${k}.png')` }}
									/>

									<span>{title.title}</span>
								</Link>
							);
						})}
					</PopoverBody>
				</Popover>
			</div>
		);
	}

	public renderGame(key: string) {
		const { identity, selectedGame } = this.props;

		const game = data.games[key];
		const titleKeys = Object.keys(game.titles);
		const titles = Boolean(titleKeys.length > 1);
		const active = key === selectedGame;

		if (!titles) {
			return (
				<Link
					className={classnames('game', { active })}
					key={key}
					to={this.replaceUrl(game.titles[titleKeys[0]].url, identity)}
				>
					<div
						className={'game-icon'}
						style={{ backgroundImage: `url('/images/switcher-bar/${key}.png')` }}
					/>
					<div className={'info'}>
						<span className={'game'}>{game.title}</span>
						<span className={'identity'}>{identity}</span>
					</div>
				</Link>
			);
		}

		const switcherTarget = `${key}-switcher-target`;

		return (
			<div
				className={classnames('game', 'multi', { active })}
				id={switcherTarget}
				key={key}
				onClick={() => this.togglePopover(key)}
			>
				<div
					className={'game-icon'}
					style={{ backgroundImage: `url('/images/switcher-bar/${key}.png')` }}
				/>
				<div className={'info'}>
					<span className={'game'}>{game.title}</span>
					<span className={'identity'}>{identity}</span>
				</div>
				{this.renderDropdown(key, switcherTarget)}
			</div>
		);
	}

	public renderSocial(key: string) {
		const social = data.socials[key];

		return (
			<div
				className={'social'}
				key={key}
			>
				<i className={`fab fa-${social.icon}`} />
			</div>
		);
	}

	public render() {
		return (
			<div className={'game-switcher-component'}>
				<Container fluid>
					<div className={'games'}>
						{Object.keys(data.games).map(k => this.renderGame(k))}
					</div>
					<div className={'socials'}>
						{Object.keys(data.socials).map(k => this.renderSocial(k))}
					</div>
				</Container>
			</div>
		);
	}

	private togglePopover = (key: string) => {
		this.setState(prevState => ({
			popovers: {
				...prevState,
				[key]: !prevState.popovers[key],
			},
		}));
	}

	private replaceUrl(url: string, identity: string) {
		return url.replace('{identity}', identity);
	}
}
