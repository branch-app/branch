const util = require('util');

const catchers = {
	debug: log,
	info: log,
	warn: log,
	error: log,
	fatal: log
};

const errorLevel = {
	debug: 0,
	info: 1,
	warn: 2,
	error: 3,
	fatal: 4
};

const statuses = {};

var minErrorLevel = 0;

function log(level, err) {
	var errorString = level + ':' + JSON.stringify(err);

	var f = level == 'fatal' ? 'error' : level;
	if (typeof console[f] === 'function')
		console[f](errorString);
	else
		console.log(errorString);
}

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
	this.reasons = reasons;
	this.meta = meta;
}

util.inherits(CuvvaError, Error);
module.exports.Error = CuvvaError;

function propagateError(level, err) {
	if (!(level in catchers) ||
		errorLevel[level] < minErrorLevel)
		return;

	catchers[level](level, err);
}

module.exports.addHandler = function (level, catcher) {
	if (typeof catcher === 'undefined') {
		catcher = level;

		if (typeof catcher !== 'function')
			throw new Error('Catcher is not a function');

		for (var prop in catchers)
			catchers[prop] = catcher;

		return;
	}

	if (typeof catcher !== 'function')
		throw new Error('Catcher is not a function');

	catchers[level] = catcher;
};

module.exports.setLevel = function (n) {
	minErrorLevel = errorLevel[n];
};

module.exports.setStatuses = function (s) {
	for (var o in s)
		statuses[o] = s[o];
};

module.exports.coerce = function (e) {
	if (e instanceof CuvvaError)
		return e;

	if (e instanceof Error)
		return new CuvvaError(e.message, { stack: e.stack });

	return new CuvvaError('unknown_runtime_error', { error: e });
};

function logger(level) {
	return function (code, reasons, meta) {
		if (code instanceof CuvvaError) {
			propagateError(level, code);
			return code;
		}

		var err = new CuvvaError(code, reasons, meta);

		propagateError(level, err);

		return err;
	};
}

module.exports.debug = logger('debug');
module.exports.info = logger('info');
module.exports.warn = logger('warn');
module.exports.error = logger('error');
module.exports.fatal = logger('fatal');
