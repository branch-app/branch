import camelize from 'camelize';
import express from 'express';
import log from '@branch-app/log';
import methodVersionMap from './import-methods';
import snakeize from 'snakeize';
import * as Middleware from './middleware';

const httpOK = 200;
const httpNoContent = 204;
const versionRegex = /^\d{4}-\d{2}-\d{2}$/;
const versions = Object.keys(methodVersionMap).sort();
const latestVersion = versions[versions.length - 1];

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
		const authorization = req.get('Authorization');

		if (!authorization)
			throw log.info('invalid_authentication');

		if (!this.app.config.internalKeys.find(k => `bearer ${k}` === authorization))
			throw log.info('invalid_authentication');

		if (req.method.toLowerCase() !== 'post')
			throw log.info('method_not_allowed');

		const methods = getMethodSet(req.params.version);
		const method = methods[req.params.method];

		if (!method)
			throw log.info('function_not_found');

		method.validator(req.body);

		const context = {
			app: this.app,
			input: camelize(req.body),
		};

		const output = await method.func(context);

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

function getMethodSet(version) {
	if (version === 'preview') {
		if (methodVersionMap.preview)
			return methodVersionMap.preview;

		throw log.info('preview_not_available');
	}

	if (version === 'latest')
		return methodVersionMap[latestVersion];

	if (!versionRegex.test(version))
		throw log.info('invalid_version');

	try {
		if (new Date(version) > new Date())
			throw log.info('version_not_found');
	} catch (error) {
		throw log.info('invalid_version');
	}

	if (methodVersionMap[version])
		return methodVersionMap[version];

	throw log.info('version_not_found');
}
