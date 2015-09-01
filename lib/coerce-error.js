module.exports = function (error) {
	if (error instanceof CuvvaError)
		return error;

	var newError;

	if (error instanceof Error) {
		newError = new CuvvaError('unknown', { message: error.message });
		Object.defineProperty(newError, 'stack', { get: function() { return error.stack } });
	} else {
		newError = new CuvvaError('unknown', { error: error });
	}

	if (error.statusCode)
		newError.httpStatus = error.statusCode;

	return newError;
};
