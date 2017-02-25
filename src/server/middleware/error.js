import errors from './error.json';
import log from 'cuvva-log';

const defaultHttpStatus = 500;

// eslint-disable-next-line no-unused-vars
export default function (origError, req, res, next) {
	let error = origError;

	if (typeof error.code !== 'string') {
		error = log.CuvvaError.coerce(error);

		log.warn('traditional_error', [error]);
	}

	res.status(errors[error.code] || defaultHttpStatus);
	res.json(error);
}
