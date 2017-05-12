import App from '../../app';
import halo4 from './halo4';
import home from './home';
import router from 'express-promise-router';

export default function (app: App) {
	const r = router();

	r.get('/', home);
	r.use('/halo-4', halo4(app));

	return r;
}
