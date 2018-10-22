import Fader from '~/Atoms/Fader';
import { Link } from 'react-router-dom';
import classnames from 'classnames';
import * as numeral from 'numeral';
import * as React from 'react';
import { Col, Container, Row } from 'reactstrap';
import './index.css';

const overview = {
	gamertag: 'PhoenixBanTrain',
	flair: {
		model: 'https://spartans.svc.halowaypoint.com/players/PhoenixBanTrain/h4/spartans/fullbody?target=large',
		emblem: 'https://emblems.svc.halowaypoint.com/h4/emblems/white_cobalt_grid-on-silver_4diamonds?size=120',
	},
	kills: 30144,
	deaths: 15123,
	gamesStarted: 1671,
};
const games = [
	{
		mapImage: '/images/games/halo-4/H4MapAssets/large/harvest.jpg',
		game: 'Slayer',
		map: 'Harvest',
		id: '12',
		score: 290,
		stat: 18,
		statTitle: 'Kills',
		kills: 18,
		medals: 20,
	},
	{
		mapImage: '/images/games/halo-4/H4MapAssets/large/ragnarok.jpg',
		game: 'CTF',
		map: 'Ragnarok',
		id: '34',
		score: 320,
		stat: 2,
		statTitle: 'Captures',
		kills: 25,
		medals: 15,
	},
	{
		mapImage: '/images/games/halo-4/H4MapAssets/large/haven.jpg',
		game: 'Slayer',
		map: 'Haven',
		id: '56',
		score: 290,
		stat: 12,
		statTitle: 'Kills',
		kills: 12,
		medals: 15,
	},
	{
		mapImage: '/images/games/halo-4/H4MapAssets/large/ravine.jpg',
		game: 'Slayer',
		map: 'Ravine',
		id: '78',
		score: 290,
		stat: 22,
		statTitle: 'Kills',
		kills: 22,
		medals: 54,
	},
	{
		mapImage: '/images/games/halo-4/H4MapAssets/large/pitfall-4d905f5d-6e0c-467c-a592-de09d41f7517.jpg',
		game: 'Slayer',
		map: 'Pitfall',
		id: '90',
		score: 290,
		stat: 8,
		statTitle: 'Kills',
		kills: 8,
		medals: 16,
	},
];

interface IState {
	interval: number|null;
	gameIndex: number;
}

export default class RecentMatchesHeader extends React.PureComponent<{}, IState> {
	constructor(props: {}) {
		super(props);

		this.state = {
			interval: null,
			gameIndex: 0,
		};

		for (const game of games)
			new Image().src = game.mapImage;
	}

	public componentDidMount() {
		const interval = window.setInterval(this.changeGame, 4500);

		this.setState({ interval });
	}

	public componentWillUnmount() {
		const { interval } = this.state;

		if (!interval)
			return;

		window.clearInterval(interval);
	}

	public renderIndexIndicator() {
		const { gameIndex } = this.state;

		return (
			<Col
				className={'indicator-index-container'}
				xs={12}
			>
				{games.map((game, i) => (
					<div
						className={classnames('indicator-item', { active: i === gameIndex })}
						key={game.id}
						onClick={() => this.selectGame(i)}
					/>
				))}
			</Col>
		);
	}

	public renderGameStatistic(title: string, subTitle: string, value: number) {
		return (
			<div className={'game-statistic'}>
				<span className={'value'}>
					{numeral(value).format('0,0')}
				</span>
				<div className={'key'}>
					<span className={'header-key'}>{title}</span>
					<span className={'sub-key'}>{subTitle}</span>
				</div>
			</div>
		);
	}

	public renderPlayerInformation() {
		return (
			<Col
				className={'player-information'}
				xs={12}
			>
				<h1>{overview.gamertag}</h1>
				<ul>
					<li>{`${numeral(overview.kills).format('0,0')} Kills`}</li>
					<li>{`${numeral(overview.deaths).format('0,0')} Deaths`}</li>
					<li>{`${numeral(overview.gamesStarted).format('0,0')} Games stated`}</li>
				</ul>
			</Col>
		);
	}

	public renderRecentGameStats() {
		const { gameIndex } = this.state;
		const game = games[gameIndex];

		return (
			<Col
				className={'recent-game-info'}
				xs={4}
			>
				<Fader
					animator={game.id}
					className={'row'}
					duration={500}
				>
					<Col
						className={'game-overview'}
						xs={9}
					>
						<span className={'game-mode'}>
							{game.game}
						</span>
						{' on '}
						<span className={'game-map'}>
							{game.map}
						</span>
					</Col>
					<Col
						className={'link-to-game'}
						xs={3}
					>
						<Link
							className={'alt'}
							to={`#${game.id}`}
						>
							{'View game'}
						</Link>
					</Col>
					<Col xs={12}><hr /></Col>
					<Col
						className={'game-stats'}
						xs={12}
					>
						{this.renderGameStatistic('Personal score', 'War games', game.score)}
						{this.renderGameStatistic(game.statTitle, game.game, game.stat)}
						{this.renderGameStatistic('Medals', 'War games', game.medals)}
					</Col>
				</Fader>
			</Col>
		);
	}

	public render() {
		const { gameIndex } = this.state;

		return (
			<section
				className={'recent-matches-header-component'}
				style={{
					backgroundImage: `url('${games[gameIndex].mapImage}')`
				}}
			>
				<div className={'mask'}>
					<Container fluid>
						<Row>
							{this.renderIndexIndicator()}
							{this.renderPlayerInformation()}
							{this.renderRecentGameStats()}
						</Row>
					</Container>
				</div>
			</section>
		);
	}

	private selectGame = (index: number) => {
		let { interval } = this.state;

		if (interval)
			window.clearInterval(interval);

		interval = window.setInterval(this.changeGame, 4500);

		this.setState({
			gameIndex: index,
			interval,
		});
	}

	private changeGame = () => {
		let { gameIndex } = this.state;

		gameIndex += 1;

		if (gameIndex > games.length - 1)
			gameIndex = 0;

		this.setState({ gameIndex });
	}
}
