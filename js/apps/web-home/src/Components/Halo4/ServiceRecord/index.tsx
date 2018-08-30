import GameSwitcher from '../../../Atoms/GameSwitcher';
import RecentGamesHeader from '../RecentMatchesHeader'
import * as React from 'react';
import './index.css';

export default class ServiceRecord extends React.PureComponent {
	public render() {
		return (
			<div className={'service-record-component'}>
				<RecentGamesHeader />
				<GameSwitcher
					selectedGame={'halo'}
					selectedTitle={'halo-4'}
					identity={'PhoenixBanTrain'}
				/>
			</div>
		);
	}
}
