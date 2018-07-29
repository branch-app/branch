import errors from './error.json';
import log from 'branch-log';
import snakecaseKeys from 'snakeize';

const httpInternalServerError = 500;

// eslint-disable-next-line no-unused-vars
export default function (error, req, res, next) {
	if (typeof error.code !== 'string') {
		// eslint-disable-next-line no-console
		console.warn(error, error.stack);

		// eslint-disable-next-line no-param-reassign
		error = log.BranchError.coerce(error);
		log.warn('traditional_error', [error]);
	}

	res.status(errors[error.code] || httpInternalServerError);
	res.json(snakecaseKeys(error));
}
