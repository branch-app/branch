import { Link } from 'react-router-dom';
import * as React from 'react';
import './index.css';

interface IState {
	year: number;
};

export default class Footer extends React.PureComponent<{}, IState> {
	constructor(props: {}) {
		super(props);

		this.state = {
			year: new Date().getFullYear(),
		};
	}

	public render() {
		const { year } = this.state;

		return (
			<footer>
				<div className={'content'}>
					<div className={'info'}>
						<p>
							{'Branch was built in '}
							<a
								className={'alt'}
								target={'_blank'}
								href={'https://www.microsoft.com/net'}
							>
								{'.NET'}
							</a>
							{', '}
							<a
								className={'alt'}
								target={'_blank'}
								href={'https://nodejs.org/'}
							>
								{'Node JS'}
							</a>
							{', and '}
							<a
								className={'alt'}
								target={'_blank'}
								href={'https://reactjs.org/'}
							>
								{'React'}
							</a>
							{'.'}
						</p>
						<p>
							{'Code licensed under the '}
							<a
								className={'alt'}
								target={'_blank'}
								href={'http://www.dbad-license.org/'}
							>
								{'tbd'}
							</a>
							{'.'}
						</p>
						<p>
							{`© Branch App, ${year}`}
						</p>

						<ul>
							<li>
								<a
									className={'alt'}
									target={'_blank'}
									href={'https://github.com/branch-app'}
								>
									Github
								</a>
							</li>
							<li>
								<Link
									className={'alt'}
									to={'/about'}
								>
									About
								</Link>
							</li>
						</ul>
					</div>
				</div>
			</footer>
		);
	}
}
