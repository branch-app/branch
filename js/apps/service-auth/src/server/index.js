import camelize from 'camelize';
import express from 'express';
import log from '@branch-app/log';
import snakeize from 'snakeize';
import * as Methods from './methods';
import * as Middleware from './middleware';

const httpOK = 200;
const httpNoContent = 204;

export default class Server {
	constructor(app, options = {}) {
		this.app = app;
		this.express = express();
		this.options = options;
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
		e.use('/1/:version/:method', wrap(this._handler, this));
		e.use(Middleware.notFound);
		e.use(Middleware.error);
	}

	_healthCheck(req, res) {
		res.sendStatus(httpNoContent);
	}

	async _handler(req, res) {
		if (req.method.toLowerCase() !== 'post')
			throw log.info('method_not_allowed');

		const method = Methods[camelize(req.params.method)];
		const date = getVersionDate(req.params.version);

		if (!method)
			throw log.info('function_not_found');

		if (date === null)
			throw log.info('preview_not_available');

		const context = {
			app: this.app,
			input: camelize(req.body),
		};

		const output = await method(context);

		if (output === void 0 || output === null) {
			res.status(httpNoContent);
			res.end();

			return;
		}

		res.status(httpOK);
		res.json(snakeize(output));
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

function getVersionDate(version) {
	switch (version) {
		case 'latest': return new Date();
		case 'preview': return null;

		default: {
			if (!(/^\d{4}-\d{2}-\d{2}$/).test(version))
				throw log.info('invalid_version');

			try {
				return new Date(version);
			} catch (error) {
				throw log.info('invalid_version');
			}
		}
	}
}
