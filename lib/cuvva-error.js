const util = require('util');

function CuvvaError(code, reasons, meta) {
	if (!(this instanceof CuvvaError))
		return new CuvvaError(code, reasons, meta);

	if (!Array.isArray(reasons)) {
		meta = reasons;
		reasons = void 0;
	}

	CuvvaError.super_(code);
	Error.captureStackTrace(this, this.constructor);

	Object.defineProperty(this, 'name', { get: function () { return 'CuvvaError'; } });
	Object.defineProperty(this, 'message', { get: function () { return this.code; } });

	this.code = code;
	this.httpStatus = CuvvaError.httpStatusMap[code] || 500;
	this.meta = meta;
	this.reasons = reasons;
}

CuvvaError.httpStatusMap = {};

util.inherits(CuvvaError, Error);
module.exports = CuvvaError;
