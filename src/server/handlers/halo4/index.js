import App from '../../../app';
import router from 'express-promise-router';
import serviceRecord from './service-record';

export default function (app: App) {
	const r = router();

	r.get('/:identity', serviceRecord.bind(null, app));
	r.get('/:identity/service-record', serviceRecord.bind(null, app));

	return r;
}
