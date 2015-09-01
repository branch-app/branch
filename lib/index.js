module.exports = function (client) {
	return function (level, err) {
		if (level == 'warn')
			level = 'warning';

		client.captureError(toJsonSerializable(err), {
			level: level,
			extra: {
				reasons: err.reasons,
				meta: err.meta
			}
		});
	};
};

function toJsonSerializable(err) {
	var output = {};

	if (err.reasons) {
		output.reasons = err.reasons.map(function (e) { return toJsonSerializable(e); });
	}

	return extractGetters(err, output, 'reasons');
}

function extractGetters(err, cur, excluding) {
	var output = cur || {};
	var props = Object.getOwnPropertyNames(err);

	for (var a in props) {
		if (excluding && excluding.indexOf(props[a]) >= 0)
			continue;

		output[props[a]] = err[props[a]];
	}

	return output;
}
