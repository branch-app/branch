module.exports = function (client) {
	return function (level, err) {
		client.captureError(err);
	};
};
