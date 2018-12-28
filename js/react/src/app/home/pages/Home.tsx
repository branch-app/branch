import { ApplicationState, ConnectedReduxProps } from '../store';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import * as React from 'react';

import Page from '../components/layout/Page';

export interface IProps extends ConnectedReduxProps { }

class Home extends React.PureComponent<IProps> {
	public render() {
		return (
			<Page>

			</Page>
		);
	}
}

const mapStateToProps = (state: ApplicationState) => ({});
const mapDispatchToProps = (dispatch: Dispatch) => ({});

export default connect(
	mapStateToProps,
	mapDispatchToProps,
)(Home);
