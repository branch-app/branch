import App from '../app';
import express from 'express';
import log from 'cuvva-log';
import Handlers from './handlers';
import * as Middleware from './middleware';

const httpStatusNoContent = 204;

export default class Server {
	app;
	express;
	handlers;

	constructor(app, options) {
		this.app = app;
		this.express = express();
		this.options = options;
		this.handlers = new Handlers(app);
		this.handlers.setup();
	}

	async setup() {
		await this._setupMiddleware();
	}

	run() {
		this.express.listen(this.options.port);

		log.info('server_listening', { port: this.options.port });
	}

	async _setupMiddleware() {
		const e = this.express;

		e.use(Middleware.types);
		e.use(Middleware.body);
		e.get('/system/health', wrap(this._healthCheck, this));
		e.use('/v1/', this.handlers.router);
		e.use(Middleware.notFound);
		e.use(Middleware.error);
	}

	_healthCheck(req, res) {
		res.sendStatus(httpStatusNoContent);
	}
}

function wrap(handler, thisArg) {
	return (req, res, next) => {
		(async () => {
			try {
				await handler.call(thisArg, req, res);

				if (!res.headersSent)
					next();
			} catch (error) {
				next(error);
			}
		})();
	};
}
