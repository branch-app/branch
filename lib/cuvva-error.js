const util = require('util');
const coerceError = require('./coerce-error');

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
