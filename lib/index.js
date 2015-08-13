var raven = require('raven');
var log =   require('cuvva-log')
var client;

// Anything below this will be logged as a message instead of an error.
// This means anything except the code will be thrown away.
var minErrorLevel = 'warn';

module.exports.init = function (dsn, level) {
	client = new raven.Client(dsn, level);
}

module.exports.default = function (level, err) {
	if (!isCuvvaError(err))
		return;

	if (log.levels[level] < log.levels[minErrorLevel])
		client.captureMessage(err.code);
	else
		client.captureError(err);
}

function isCuvvaError(err) {
	if (!('code' in err))
		return false;

	for (var prop in err)
		if (prop !== 'meta' &&
			prop !== 'code' &&
			prop !== 'reasons')
			return false;

	return true;
}
