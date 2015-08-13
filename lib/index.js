module.exports = function (client, level) {
	return function (level, err) {
		client.captureError(err, { level: level });
	};
};
