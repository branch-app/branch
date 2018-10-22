import { ReduxAction } from "./redux";

export type IdentityAction = (type: XboxLiveIdentityType, value: string | number) => ReduxAction<IdentityPayload>;

export interface IdentityPayload {
	type: XboxLiveIdentityType;
	value: string|number;
}

export enum XboxLiveIdentityType {
	Gamertag = 'gamertag',
	XUID = 'xuid',
}
