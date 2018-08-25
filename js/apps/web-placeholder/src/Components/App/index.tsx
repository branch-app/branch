import * as React from 'react';
import './index.css';

interface IState {
	frontIndex: number;
	backIndex: number;
	interval: number|null;
	images: string[];
}

class App extends React.PureComponent<{}, IState> {
	public constructor(props: {}) {
		super(props);

		this.state = {
			backIndex: -1,
			frontIndex: 0,
			interval: null,

			images: [
				'halo-3',
				'halo-4',
				'halo-5',
			],
		};

		for (const image of this.state.images)
			new Image().src = `/images/${image}.jpg`;
	}

	public componentDidMount() {
		const interval = window.setInterval(this.handleImageChange, 3500);

		this.setState({ interval });
	}

	public componentWillUnmount() {
		const { interval } = this.state;

		if (!interval)
			return;
		
		window.clearInterval(interval);
	}

	public render() {
		const { backIndex, frontIndex, images } = this.state;

		return (
			<div className={'component-app'}>
				<div
					className={'back-image'}
					style={{
						backgroundImage: `url(/images/${images[backIndex]}.jpg)`
					}}
				/>
				<div
					className={'front-image'}
					style={{
						backgroundImage: `url(/images/${images[frontIndex]}.jpg)`
					}}
				/>
				
				<div className={'container'}>
					<div className={'content'}>
						<h1>{'Branch app'}</h1>
						<hr />
						<h2>
							{'Coming soon '}
							<span className={'emoji'}>{'👀'}</span>
						</h2>
					</div>
				</div>
			</div>
		);
	}

	private handleImageChange = () => {
		let { frontIndex, backIndex } = this.state;

		backIndex = frontIndex;
		frontIndex += 1;

		if (frontIndex > 2)
			frontIndex = 0;
		
		this.setState({ frontIndex, backIndex });
	}
}

export default App;
