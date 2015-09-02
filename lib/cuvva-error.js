const util = require('util');

function CuvvaError(code, reasons, meta) {
	if (!(this instanceof CuvvaError))
		return new CuvvaError(code, reasons, meta);

	if (!meta && !Array.isArray(reasons)) {
		meta = reasons;
		reasons = void 0;
	}

	for (var i in reasons) {
		if (!(reasons[i] instanceof CuvvaError))
			reasons[i] = CuvvaError.coerce(reasons[i]);
	}

	CuvvaError.super_(code);
	Error.captureStackTrace(this, this.constructor);

	const statusCode = CuvvaError.httpStatusMap[code] || 500;

	Object.defineProperty(this, 'name', { get: function () { return 'CuvvaError'; } });
	Object.defineProperty(this, 'message', { get: function () { return this.code; } });
	Object.defineProperty(this, 'httpStatus', { get: function () { return statusCode; } });

	this.code = code;
	this.reasons = reasons;
	this.meta = meta;
}

util.inherits(CuvvaError, Error);
module.exports = CuvvaError;

CuvvaError.httpStatusMap = {};

CuvvaError.coerce = function (error) {
	if (error instanceof CuvvaError)
		return error;

	var newError;

	if (typeof error.code === 'string') {
		newError = new CuvvaError(error.code, error.reasons, error.meta);
	} else if (error instanceof Error) {
		newError = new CuvvaError('unknown', { message: error.message });
		Object.defineProperty(newError, 'stack', { get: function () { return error.stack } });
	} else {
		newError = new CuvvaError('unknown', { error: error });
	}

	const httpStatus = error.httpStatus || error.statusCode || error.status;

	if (typeof httpStatus === 'number')
		Object.defineProperty(newError, 'httpStatus', { get: function () { return httpStatus; } });

	return newError;
};
