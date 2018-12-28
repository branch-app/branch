import { IdentityType } from '../types/identity-type';

export interface IdentityState {

}

export interface IdentityPayload {
	type: 'gamertag'|'xuid';
	value: string;
}

export const IdentityActionTypes = {

}
