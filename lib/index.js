const CuvvaError = require('./cuvva-error');

module.exports.CuvvaError = CuvvaError;

const errorLevels = {
	debug: 0,
	info: 1,
	warn: 2,
	error: 3,
	fatal: 4,
};

const config = {
	handlers: {},
	minLogLevel: null,
};

// default config
for (var level in errorLevels)
	config.handlers[level] = defaultHandler;

config.minLogLevel = errorLevels.info;

function defaultHandler(level, error) {
	const message = level + ':' + JSON.stringify(error);
	const method = level == 'fatal' ? 'error' : level;

	if (typeof console[method] === 'function')
		console[method](message);
	else
		console.log(message);
}

function handleError(level, error) {
	if (errorLevels[level] < config.minLogLevel)
		return;

	if (level === 'fatal' && config.handlers[level] !== defaultHandler)
		defaultHandler(level, error);

	config.handlers[level](level, error);
}

module.exports.setHandler = function (level, handler) {
	if (handler === void 0 && typeof level === 'function') {
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

function logger(level) {
	return function (code, reasons, meta) {
		if (code instanceof CuvvaError) {
			handleError(level, code);
			return code;
		}

		const error = new CuvvaError(code, reasons, meta);
		handleError(level, error);
		return error;
	};
}

module.exports.debug = logger('debug');
module.exports.info = logger('info');
module.exports.warn = logger('warn');
module.exports.error = logger('error');
module.exports.fatal = logger('fatal');
