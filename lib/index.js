const util = require('util');

const errorLevels = {
	debug: 0,
	info: 1,
	warn: 2,
	error: 3,
	fatal: 4
};

const config = {
	handlers: {},
	httpStatusMap: {},
	minLogLevel: null
};

// default config
for (var level in errorLevels)
	config.handlers[level] = defaultHandler;

config.httpStatusMap.unknown = 500;
config.minLogLevel = errorLevels.info;

function defaultHandler(level, error) {
	if (errorLevels[level] < config.minLogLevel)
		return;

	var message = level + ':' + JSON.stringify(error);

	var method = level == 'fatal' ? 'error' : level;
	if (typeof console[method] === 'function')
		console[method](message);
	else
		console.log(message);
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
	this.httpStatus = config.httpStatusMap[code];
	this.meta = meta;
	this.reasons = reasons;
}

util.inherits(CuvvaError, Error);
module.exports.Error = CuvvaError;

function handleError(level, error) {
	if (!config.handlers[level])
		return;

	config.handlers[level](level, error);
}

module.exports.setHandler = function (level, handler) {
	if (typeof handler === 'undefined' && typeof level === 'function') {
		handler = level;
		level = null;
	}

	if (typeof handler !== 'function')
		throw new Error('Handler is not a function');

	if (level) {
		config.handlers[level] = handler;
	} else {
		for (var prop in config.handlers)
			config.handlers[prop] = handler;
	}
};

module.exports.setMinLogLevel = function (level) {
	config.minLogLevel = errorLevels[level];
};

module.exports.setHttpStatuses = function (statusMap) {
	for (var code in statusMap)
		config.httpStatusMap[code] = statusMap[code];
};

module.exports.coerceError = function (error) {
	if (error instanceof CuvvaError)
		return error;

	if (error instanceof Error)
		return new CuvvaError('unknown', { message: error.message });

	return new CuvvaError('unknown', { error: error });
};

function logger(level) {
	return function (code, reasons, meta) {
		var error = new CuvvaError(code, reasons, meta);
		handleError(level, error);
		return error;
	};
}

module.exports.debug = logger('debug');
module.exports.info = logger('info');
module.exports.warn = logger('warn');
module.exports.error = logger('error');
module.exports.fatal = logger('fatal');
