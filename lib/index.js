module.exports = function (client) {
	return function (level, err) {
		if (level == 'warn')
			level = 'warning';

		client.captureError(err, {
			level: level,
			extra: {
				reasons: err.reasons,
				meta: err.meta
			}
		});
	};
};
