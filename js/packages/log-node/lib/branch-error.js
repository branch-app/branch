const util = require('util');

function BranchError(code, reasons, meta) {
	if (!(this instanceof BranchError))
		return new BranchError(code, reasons, meta);

	if (!meta && !Array.isArray(reasons)) {
		meta = reasons;
		reasons = void 0;
	}

	for (var i in reasons) {
		if (!(reasons[i] instanceof BranchError))
			reasons[i] = BranchError.coerce(reasons[i]);
	}

	BranchError.super_(code);
	Error.captureStackTrace(this, this.constructor);

	this.code = code;
	this.reasons = reasons;
	this.meta = meta;

	defineNonSerializable(this, 'name', 'BranchError');
	defineNonSerializable(this, 'message', this.code);
}

util.inherits(BranchError, Error);
module.exports = BranchError;

BranchError.coerce = function (error) {
	if (error instanceof BranchError)
		return error;

	var newError;

	if (typeof error.code === 'string') {
		// if code is a string, obj already conforms to BranchError structure
		newError = new BranchError(error.code, error.reasons, error.meta);
	} else if (error instanceof Error) {
		// if obj is an Error, pull out its useful details
		newError = new BranchError('unknown', { message: error.message });
	} else {
		// worst case, just provide obj in the meta fields
		newError = new BranchError('unknown', { error: error });
	}

	if (error instanceof Error)
		defineNonSerializable(newError, 'stack', error.stack);

	return newError;
};

function defineNonSerializable(obj, property, value) {
	Object.defineProperty(obj, property, {
		value: value,
		writable: false,
		enumerable: false, // prevents json serialization
		configurable: true, // allows redefinition
	});
}
