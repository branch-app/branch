module.exports = function (client) {
	return function (level, err) {
		client.captureError(err, {
			level: level,
			extra: {
				reasons: err.reasons,
				meta: err.meta
			}
		});
	};
};
