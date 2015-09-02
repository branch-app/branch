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

	// non-enumerable properties prevent json serialization
	Object.defineProperty(this, 'name', { value: 'CuvvaError' });
	Object.defineProperty(this, 'message', { value: this.code });
	Object.defineProperty(this, 'httpStatus', { value: statusCode });

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
		// if code is a string, obj already conforms to CuvvaError structure
		newError = new CuvvaError(error.code, error.reasons, error.meta);
	} else if (error instanceof Error) {
		// if obj is an Error, pull out its useful details
		newError = new CuvvaError('unknown', { message: error.message });
		// non-enumerable property prevents json serialization
		Object.defineProperty(newError, 'stack', { value: error.stack });
	} else {
		// worst case, just provide obj in the meta fields
		newError = new CuvvaError('unknown', { error: error });
	}

	const httpStatus = error.httpStatus || error.statusCode || error.status;

	if (typeof httpStatus === 'number')
		Object.defineProperty(newError, 'httpStatus', { value: httpStatus });

	return newError;
};
