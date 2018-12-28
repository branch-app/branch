export interface IdentityState {

}

export interface IdentityPayload {
	type: 'gamertag'|'xuid';
	value: string;
}

export const IdentityActionTypes = {
	FETCH_IDENTITY: '@@platform/identity/FETCH_IDENTITY',
	FETCHED_IDENTITY_SUCCESS: '@@platform/identity/FETCHED_IDENTITY_SUCCESS',
	FETCHED_IDENTITY_FAILURE: '@@platform/identity/FETCHED_IDENTITY_FAILURE',
}
