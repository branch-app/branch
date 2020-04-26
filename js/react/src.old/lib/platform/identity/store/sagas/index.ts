import fetchIdentity from './fetch-identity';
import { all, fork } from 'redux-saga/effects';

export default function* identitySaga() {
	yield all([
		fork(fetchIdentity),
	]);
}
