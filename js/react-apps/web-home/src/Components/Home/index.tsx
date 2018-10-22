import Typed from 'typed.js';
import * as React from 'react';
import {
	Button,
	Col,
	Container,
	Input,
	InputGroup,
	InputGroupAddon,
	Row,
} from 'reactstrap';
import './index.css';
import { push } from 'connected-react-router';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';

export interface IProps {
	dispatch: Dispatch<any>;
}

interface IState {
	interval: number|null;
	searchContent: string;
	frontIndex: number;
	backIndex: number;

	images: string[];
}

class Home extends React.PureComponent<IProps, IState> {
	private typed: Typed;

	constructor(props: IProps) {
		super(props);

		this.state = {
			interval: null,
			frontIndex: 0,
			backIndex: 0,
			searchContent: '',

			images: [
				'halo-3',
				'halo-3-odst',
				'halo-4',
				'halo-5',
				'halo-reach',
				'destiny',
			],
		};

		for (const image of this.state.images)
			new Image().src = `/images/home/${image}.jpg`;
	}

	public componentDidMount() {
		const interval = window.setInterval(this.changeImage, 4500);

		this.setState({ interval });

		const options = {
			attr: 'placeholder',
			typeSpeed: 50,
			backSpeed: 30,
			strings: [
				'PhoenixBanTrain',
				'Program',
				'Major Nelson',
				'Any gamertag you like ✨',
			],
		};

		this.typed = new Typed('.search-input', options);
	}

	public componentWillUnmount() {
		const { interval } = this.state;

		if (this.typed)
			this.typed.destroy();

		if (!interval)
			return;

		window.clearInterval(interval);
	}

	public render() {
		const { backIndex, frontIndex, images } = this.state;

		return (
			<div className={'component-home'}>
				<section className={'hero'}>
					<div
						className={'back-image'}
						style={{
							backgroundImage: `url('/images/home/${images[backIndex]}.jpg')`
						}}
					/>
					<div
						className={'front-image'}
						style={{
							backgroundImage: `url('/images/home/${images[frontIndex]}.jpg')`
						}}
					/>

					<div className={'content'}>
						<div className={'info'}>
							<h1>{'Branch'}</h1>
							<h2>{'Xbox Live, Halo, and Destiny stats - made easy'}</h2>

							<div className={'searcher'}>
								<p>{'Looking for someone? 🕵🏻‍'}</p>

								<InputGroup>
									<Input
										className={'search-input'}
										onChange={this.searchChanged}
										onKeyDown={this.searchKeyDown}
										value={this.state.searchContent}
									/>
									<InputGroupAddon addonType={'append'}>
										<Button
											color={'primary'}
											onClick={this.searchPressed}
										>
											{'Search'}
										</Button>
									</InputGroupAddon>
								</InputGroup>
							</div>
						</div>
					</div>
				</section>

				<div className={'loren-bar'}>
					<Container>
						<Row>
							<Col>
								<img
									className={'rounded-circle'}
									src={'https://placemiliband.uk/280/280'}
									alt={'Cached stats'}
									width={'140'}
									height={'140'}
								/>
								<span>{'Cached'}</span>
								<p>
									{'Stats are cached for quicker viewing. Also even '}
									{'if third party data-sources are down, cached '}
									{'stats are still available here 🔥'}</p>
							</Col>
							<Col>
								<img
									className={'rounded-circle'}
									src={'https://placemiliband.uk/280/280'}
									alt={'Open source'}
									width={'140'}
									height={'140'}
								/>
								<span>{'Open source'}</span>
								<p>
									{'Completely open sourced - missing a feature? '}
									{'Submit an '}
									<a className={'alt'} href={'#'}>{'issue'}</a>{' '}
									{'requesting it, or a '}
									<a className={'alt'} href={'#'}>{'pull-request'}</a>{' '}
									{'implementing it.'}
								</p>
							</Col>
							<Col>
								<img
									className={'rounded-circle'}
									src={'https://placemiliband.uk/280/280'}
									alt={'Leaderboards'}
									width={'140'}
									height={'140'}
								/>
								<span>{'Leaderboards'}</span>
								<p>
									{'See how you\'re doing against your friends, '}
									{'boss, or the worlds best players 📈'}
								</p>
							</Col>
						</Row>
					</Container>
				</div>
			</div>
		);
	}

	private changeImage = () => {
		let { backIndex, frontIndex, images } = this.state;

		backIndex = frontIndex;
		frontIndex += 1;

		if (frontIndex > images.length - 1)
			frontIndex = 0;

		this.setState({ backIndex, frontIndex, images });
	}

	private searchKeyDown = (event: React.KeyboardEvent) => {
		if (event.key == 'Enter')
			this.searchPressed();
	}

	private searchChanged = (event: React.FormEvent<HTMLInputElement>) => {
		this.setState({ searchContent: event.currentTarget.value });
	}

	private searchPressed = () => {
		// TODO(0xdeafcafe): Validate state

		this.props.dispatch(push(`/halo-4/${this.state.searchContent}`));
	}
}

export default connect()(Home);
