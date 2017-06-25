import errors from './error.json';
import log from '@branch-app/log';
import snakecaseKeys from 'snakeize';

export default function (error, req, res, next) {
	typeof next; // handles linting issue

	if (typeof error.code !== 'string') {
		console.warn(error, error.stack);

		error = log.BranchError.coerce(error);
		log.warn('traditional_error', [error]);
	}

	// eslint-disable-next-line no-magic-numbers
	res.status(errors[error.code] || 500);
	res.json(snakecaseKeys(error));
}
