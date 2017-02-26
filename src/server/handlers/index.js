import Router from 'express-promise-router';
import * as xboxLive from './xbox-live';

const httpStatusOK = 200;
const httpStatusNoContent = 204;

export default class Handlers {
	app;
	router;

	constructor(app) {
		this.app = app;
		this.router = Router();
	}

	setup() {
		// prefix /v1/
		this.router.get('/xbox-live', this._wrap(this._handler, xboxLive.index, this));
	}

	async _handler(req, res, method) {
		const context = {
			app: this.app,
			input: req.body
		};

		const output = await method(context);

		if (output === void 0 || output === null) {
			res.status(httpStatusNoContent);
			res.end();

			return;
		}

		res.status(httpStatusOK);
		res.json(output);
	}

	_wrap(handler, method, thisArg) {
		return (req, res, next) => {
			(async () => {
				try {
					await handler.call(thisArg, req, res, method);

					if (!res.headersSent)
						next();
				} catch (error) {
					next(error);
				}
			})();
		};
	}
}
