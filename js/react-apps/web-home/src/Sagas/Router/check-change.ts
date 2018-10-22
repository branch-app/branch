import { takeEvery } from 'redux-saga/effects';
import { LOCATION_CHANGE, RouterState } from 'connected-react-router';
import { ReduxAction } from '~/Types/redux';
import { delay } from 'redux-saga';

export default function* watchLocationChange() {
	yield takeEvery(LOCATION_CHANGE, locationChange);
}

function* locationChange(action: ReduxAction<RouterState>) {
	yield delay(1);
}
