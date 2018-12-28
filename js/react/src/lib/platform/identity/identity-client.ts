import BranchClient from '../common/branch-client';
import { IdentityPayload } from './store/types';
import { XboxLiveIdentity } from './types/xboxlive-identity';
import { toSlug } from '../../shared/helpers';

export default class IdentityClient extends BranchClient {
	public async getXboxLiveIdentity(identity: IdentityPayload): Promise<XboxLiveIdentity> {
		return await this.getWithCache<XboxLiveIdentity>(
			`${identity.type}-${toSlug(identity.value)}-identity`,
			'1/2018-08-19/get_xboxlive_identity',
			identity,
		);
	}
}
