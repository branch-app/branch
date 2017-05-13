import App from '../app';
import express from 'express';
import handlebars from 'express-handlebars';
import handlers from './handlers';
import helpers from './helpers';
import log from '@branch-app/log';
import * as middleware from './middleware';

const httpNoContent = 204;

export default class Server {
	app: App;
	handlers;
	express;

	constructor(app: App, options: {} = {}) {
		this.app = app;
		this.handlers = handlers(this.app);
		this.express = express();
		this.options = options;
	}

	async setup(): Promise<void> {
		await this._setupMiddleware();
	}

	run(): void {
		this.express.listen(this.options.port);

		log.info('server_listening', { port: this.options.port });
	}

	async _setupMiddleware(): Promise<void> {
		const e = this.express;

		e.engine('.hbs', handlebars({
			defaultLayout: 'main',
			extname: '.hbs',
			helpers,
		}));
		e.set('view engine', '.hbs');

		e.use(middleware.timer);
		e.use(middleware.body);
		e.use('/public', express.static('public'));
		e.get('/system/health', this._healthCheck);
		e.use(middleware.locals);
		e.use('/', this.handlers);
		e.use(middleware.notFound);
	}

	_healthCheck(req, res): void {
		res.sendStatus(httpNoContent);
	}
}
