import { takeEvery, getContext, call, put } from 'redux-saga/effects';
import { ReduxAction } from '~/Types/redux';
import { FETCH_HALO4_SERVICE_RECORD } from '~/Constants/action-types';
import { IdentityPayload } from '~/Types/actions';
import BranchClient from '~/Clients/branch-client';
import CacheClient from '~/Clients/cache-client';

const path = '1/2018-09-12/get_service_record';

export default function* watchFetchServiceRecord() {
	yield takeEvery(FETCH_HALO4_SERVICE_RECORD, fetchServiceRecord);
}

function* fetchServiceRecord(action: ReduxAction<IdentityPayload>) {
	let serviceRecord: Halo4.ServiceRecordResponse | null = null;
	const cacheClient: CacheClient = yield getContext('cacheClient');
	const branchClient: BranchClient = yield getContext('halo4Client');

	const body = { identity: action.payload };
	const cacheKey = `h4-sr-${body.identity.type}-${body.identity.value}`;

	serviceRecord = yield call([cacheClient, cacheClient.readValue], cacheKey);

	if (serviceRecord == null) {
		serviceRecord = yield call([branchClient, branchClient.execute], path, body);

		yield call([cacheClient, cacheClient.writeValue], cacheKey, serviceRecord!);
	}

	yield put({
		type: `${FETCH_HALO4_SERVICE_RECORD}_SUCCESS`,
		payload: {
			ident: serviceRecord!.identity.xuid,
			response: serviceRecord,
		},
	});

	// TODO(0xdeafcafe): Load identities
}
