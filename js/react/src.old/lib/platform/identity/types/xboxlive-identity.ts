import { Response } from '../../common/types';

export interface XboxLiveIdentity extends Response {
	gamertag: string;
	xuid: number;
}
