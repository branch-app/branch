import { fetchIdentity } from '../actions';
import IdentityClient from '../../client';
import { PayloadAction } from 'typesafe-actions/dist/types';
import { XboxLiveIdentity } from '../../types';
import { IdentityActionTypes, IdentityPayload } from '../types';
import { call, put, takeEvery, getContext } from 'redux-saga/effects';

export default function* watchFetchIdentity() {
	yield takeEvery(IdentityActionTypes.FETCH_IDENTITY, authenticate);
}

function* authenticate({ payload }: PayloadAction<string, IdentityPayload>) {
	const client: IdentityClient = yield getContext('identityClient');

	try {
		const response: XboxLiveIdentity = yield call([client, client.getXboxLiveIdentity], payload);

		yield put(fetchIdentity.success(response));
	} catch (error) {
		// TODO(0xdeafcafe): Convert exception to BranchError.. but where?

		yield put(fetchIdentity.failure(error));
	}
}
