import App from '../../app';
import home from './home';
import router from 'express-promise-router';

export default function (app: App) {
	const r = router();

	r.get('/', home);

	return r;
}
