import BranchClient from '../shared/branch-client';
import { IdentityPayload } from '../identity/store/types';
// import { IdentityPayload } from '../Types/actions';

export default class Halo4Client extends BranchClient {
	public async getServiceRecord(identity: IdentityPayload): Promise<Halo4.ServiceRecordResponse> {
		return await this.getWithCache<Halo4.ServiceRecordResponse>(
			`${identity.type}-${identity.value}-halo4-servicerecord`,
			'1/2018-09-12/get_service_record',
			{ identity },
		);
	}

	public async getRecentMatches(identity: IdentityPayload, gameMode: Common.GameMode, count: number, startAt: number): Promise<Halo4.RecentMatches> {
		return await this.getWithCache<Halo4.RecentMatches>(
			`${identity.type}-${identity.value}-${gameMode}-${count}-${startAt}-halo4-recent-matches`,
			'1/2018-09-12/get_recent_matches',
			{ identity, gameMode, count, startAt },
		);
	}
}
