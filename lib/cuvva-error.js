const util = require('util');

function CuvvaError(code, reasons, meta) {
	if (!(this instanceof CuvvaError))
		return new CuvvaError(code, reasons, meta);

	if (!Array.isArray(reasons)) {
		meta = reasons;
		reasons = void 0;
	}

	for (var i in reasons) {
		if (!(reasons[i] instanceof CuvvaError))
			reasons[i] = coerceError(reasons[i]);
	}

	CuvvaError.super_(code);
	Error.captureStackTrace(this, this.constructor);

	const statusCode = CuvvaError.httpStatusMap[code] || 500;

	Object.defineProperty(this, 'name', { get: function () { return 'CuvvaError'; } });
	Object.defineProperty(this, 'message', { get: function () { return this.code; } });
	Object.defineProperty(this, 'httpStatus', { get: function () { return statusCode; } });

	this.code = code;
	this.meta = meta;
	this.reasons = reasons;
}

CuvvaError.httpStatusMap = {};

util.inherits(CuvvaError, Error);
module.exports = CuvvaError;

var coerceError =
module.exports.coerce =
function (error) {
	if (error instanceof CuvvaError)
		return error;

	var newError;

	if (error instanceof Error) {
		newError = new CuvvaError('unknown', { message: error.message });
		Object.defineProperty(newError, 'stack', { get: function () { return error.stack; } });
	} else {
		newError = new CuvvaError('unknown', { error: error });
	}

	if (error.statusCode)
		newError.httpStatus = error.statusCode;

	return newError;
};
