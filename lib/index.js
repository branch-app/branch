const CuvvaError = require('./cuvva-error');

module.exports.CuvvaError = CuvvaError;

const errorLevels = {
	debug: 0,
	info: 1,
	warn: 2,
	error: 3,
	fatal: 4
};

const config = {
	handlers: {},
	minLogLevel: null
};

// default config
for (var level in errorLevels)
	config.handlers[level] = defaultHandler;

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

function handleError(level, error) {
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
		CuvvaError.httpStatusMap[code] = statusMap[code];
};

module.exports.coerceError = function (error) {
	if (error instanceof CuvvaError)
		return error;

	var newError;

	if (error instanceof Error)
		newError = new CuvvaError('unknown', { message: error.message });
	else
		newError = new CuvvaError('unknown', { error: error });

	if (error.statusCode)
		newError.httpStatus = error.statusCode;

	return newError;
};

function logger(level) {
	return function (code, reasons, meta) {
		if (code instanceof CuvvaError) {
			handleError(level, code);
			return code;
		}

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
