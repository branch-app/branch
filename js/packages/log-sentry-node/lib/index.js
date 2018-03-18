module.exports = function (client) {
	return function (level, err) {
		if (level == 'warn')
			level = 'warning';

		client.captureException(err, {
			level: level,
			extra: toJsonSerializable(err)
		});
	};
};

function toJsonSerializable(err) {
	const output = {};

	if (err.reasons)
		output.reasons = err.reasons.map(function (e) { return toJsonSerializable(e); });

	return extractGetters(err, output, 'reasons');
}

function extractGetters(err, cur, excluding) {
	const output = cur || {};
	const props = Object.getOwnPropertyNames(err);

	for (var a in props) {
		if (excluding && excluding.includes(props[a]))
			continue;

		output[props[a]] = err[props[a]];
	}

	return output;
}
