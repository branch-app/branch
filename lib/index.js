const responses = {};

const catchers  = {
	debug:  log,
	info:   log,
	warn:   log,
	error:  log,
	fatal:  log,
};

function logger(level) {
	return function (code, reasons, meta) {
		var err = new CuvvaError(code, reasons, meta);

		propagateError(level, err);

		return err;
	};
}

function log(level, err) {
	// Pad so the codes of error/fatal line up with that of info and warn
	// TODO: print out inner errors instead of swallowing them (just verbose mode?)
	//       possibly not necessary as the errors are likely created with log.error anyway(?)
	var errorString = message(level, err);

	level = level == 'fatal' ? 'error' : level;
	if (level in console) {
		console[level](errorString);
	}
	else {
		console.log(errorString);
	}
}

function message(level, err) {
	var errorString = pad(level, 5) + ': ' + err.code;
	const desc = err.meta;

	if (desc)
		errorString += '\n' + JSON.stringify(desc);

	return errorString;
}

function pad(str, len) {
	if (str.length < len)
		str += new Array(len - str.length + 1).join(' ');

	return str;
}

function description(err) {
	const response = responses[err.code];

	if (typeof response === 'undefined')
		return '';

	return response(err.meta);
}


function CuvvaError(code, reasons, meta) {
	if (!(this instanceof CuvvaError))
		return new CuvvaError(code, reasons, meta);

	if (!Array.isArray(reasons)) {
		meta = reasons;
		reasons = void 0;
	}

	this.name = void 0;
	this.message = void 0;

	this.code = code;
	this.reasons = reasons;
	this.meta = meta;
}

CuvvaError.prototype = Object.create(Error.prototype, {
	constructor: { value: CuvvaError }
});

function propagateError(level, err) {
	if (!(level in catchers))
		return;

	catchers[level](level, err);
}

function addCatch(level, catcher) {
	if (typeof catcher === 'undefined') {
		catcher = level;

		if (typeof catcher !== 'function')
			throw logger('error')('register-catch-error', { catcher: catcher });

		for (var prop in catchers)
			catchers[prop] = catcher;

		return;
	}

	if (typeof catcher !== 'function')
		throw logger('error')('register-catch-error', { catcher: catcher });

	catchers[level] = catcher;
}

module.exports = {
	debug:   logger('debug'),
	info:    logger('info'),
	warn:    logger('warn'),
	error:   logger('error'),
	fatal:   logger('fatal'),
	handler: addCatch,
	create:  CuvvaError
};
