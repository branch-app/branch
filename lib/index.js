module.exports.default = function (client) {
	return function (level, err) {
		client.captureError(err);
	}
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
