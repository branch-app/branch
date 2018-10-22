import halo4 from './Halo4';
import router from './Router';
import { all } from 'redux-saga/effects';

export default function* rootSaga() {
	yield all([
		...halo4,
		...router,
	]);
}
