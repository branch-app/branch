const log = require('@branch-app/log');

/*
	type Version = {
		date: Date,
		func: Function,
	};

	type Method = {
		removedAt: Date|null,
		preview: Function|null,
		versions: Version[], // ordered newer-to-older
	};
*/
const dateRegex = /^\d{4}-\d{2}-\d{2}$/;

function generateMethodMap(methodExports) {
	const methods = {};
	const versionMap = {};
	const allVersions = new Set();

	for (const method of Object.values(methodExports)) {
		const removedAt = checkDate(method.removedAt);
		const preview = method.preview || null;
		const versions = [];

		if (preview && typeof preview !== 'function')
			throw log.warn('invalid_export', { method });

		for (const version of Object.keys(method.versions)) {
			const date = checkDate(version);
			const impl = method.versions[version];

			if (!date || typeof impl !== 'object')
				throw log.warn('invalid_export', { method });

			if (!impl.validator || typeof impl.func !== 'function')
				throw log.warn('invalid_implementation', { method });

			versions.push({ date, impl });
			allVersions.add(date);
		}

		versions.sort((a, b) => b.date - a.date);

		methods[method.name] = { removedAt, preview, versions };
	}

	for (const versionStr of Array.from(allVersions).sort()) {
		versionMap[versionStr] = {};
		const requestedDate = new Date(versionStr);

		for (const methodName of Object.keys(methods)) {
			const method = methods[methodName];

			const versions = method.versions;
			const sortedVersions = versions.sort((a, b) => new Date(b.date) - new Date(a.date));

			const implementation = sortedVersions.find(v => {
				if (method.removedAt && new Date(method.removedAt) <= requestedDate)
					return false;

				return new Date(v.date) <= requestedDate;
			});

			if (implementation)
				versionMap[versionStr][methodName] = implementation.impl;
		}
	}

	return versionMap;
}

function checkDate(str) {
	if (!str)
		return null;

	if (!dateRegex.test(str))
		throw log.warn('invalid_date', { date: str });

	const val = Date.parse(str);

	if (isNaN(val))
		throw log.warn('invalid_date', { date: str });

	return str;
}

module.exports = generateMethodMap;
