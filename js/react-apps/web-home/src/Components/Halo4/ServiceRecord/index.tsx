import GameSwitcher from '~/Atoms/GameSwitcher';
import RecentGamesHeader from '../RecentMatchesHeader';
import { connect } from 'react-redux';
import * as React from 'react';
import { Dispatch } from 'redux';
import { IdentityAction, XboxLiveIdentityType } from '~/Types/actions';
import fetchServiceRecord from '~/Actions/Halo4/fetch-service-record';
import './index.css';

const meta = {
	selectedGame: 'halo',
	selectedTitle: 'halo-4',
};

export interface IProps {
	fetchServiceRecord: IdentityAction;
	children?: React.ReactNode;
}

interface IState {

}

class ServiceRecord extends React.PureComponent<IProps, IState> {
	public componentDidMount() {
		this.props.fetchServiceRecord(XboxLiveIdentityType.Gamertag, 'phoenixbantrain');
	}

	public render() {
		return (
			<div className={'service-record-component'}>
				<RecentGamesHeader />
				<GameSwitcher
					selectedGame={meta.selectedGame}
					selectedTitle={meta.selectedTitle}
					identity={'PhoenixBanTrain'}
				/>
				{this.props.children}
			</div>
		);
	}
}

const mapStateToProps = (state: any) => ({

});

const mapDispatchToProps = (dispatch: Dispatch) => ({
	fetchServiceRecord: (type: XboxLiveIdentityType, value: string|number) => dispatch(fetchServiceRecord(type, value)),
});

export default connect(mapStateToProps, mapDispatchToProps)(ServiceRecord);
