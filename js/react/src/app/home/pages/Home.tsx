import { ApplicationState, ConnectedReduxProps } from '../store';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import * as React from 'react';

import Page from '../components/layout/Page';
import { IdentityPayload, fetchIdentity } from '../../../lib/platform/identity/store';

export interface IProps extends ConnectedReduxProps {
	fetchIdentity: typeof fetchIdentity.request,
}

class Home extends React.PureComponent<IProps> {
	public render() {
		this.props.fetchIdentity({ type: 'gamertag', value: 'phoenixbantrain' });

		return (
			<Page>

			</Page>
		);
	}
}

const mapStateToProps = (state: ApplicationState) => ({ });
const mapDispatchToProps = (dispatch: Dispatch) => ({
	fetchIdentity: (payload: IdentityPayload) => dispatch(fetchIdentity.request(payload)),
});

export default connect(
	mapStateToProps,
	mapDispatchToProps,
)(Home);
