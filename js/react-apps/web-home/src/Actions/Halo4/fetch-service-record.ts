import { ReduxAction } from "~/Types/redux";
import { IdentityPayload, XboxLiveIdentityType } from "~/Types/actions";
import { FETCH_HALO4_SERVICE_RECORD } from "~/Constants/action-types";

export default (type: XboxLiveIdentityType, value: string|number): ReduxAction<IdentityPayload> => ({
	type: FETCH_HALO4_SERVICE_RECORD,
	payload: {
		type,
		value,
	},
});
