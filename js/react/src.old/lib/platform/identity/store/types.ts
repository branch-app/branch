import { AsyncState } from '../../../shared/types';
import { XboxLiveIdentity } from '../types';

export interface IdentityState {
	gamertagMap: Record<string, string>,
	xuidMap: Record<string, string>,
	map: Record<string, AsyncState<XboxLiveIdentity>>,
}

export const initialIdentityState: IdentityState = {
	gamertagMap: {},
	xuidMap: {},
	map: {},
};

export interface IdentityPayload {
	type: 'gamertag'|'xuid';
	value: string;
}

export const IdentityActionTypes = {
	FETCH_IDENTITY: '@@platform/identity/FETCH_IDENTITY',
	FETCHED_IDENTITY_SUCCESS: '@@platform/identity/FETCHED_IDENTITY_SUCCESS',
	FETCHED_IDENTITY_FAILURE: '@@platform/identity/FETCHED_IDENTITY_FAILURE',
}
