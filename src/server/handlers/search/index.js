import App from '../../../app';
import results from './results';
import router from 'express-promise-router';

export default function (app: App) {
	const r = router();

	r.get('/:identity', results.bind(null, app));

	return r;
}
