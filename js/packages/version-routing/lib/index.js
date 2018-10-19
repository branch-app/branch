const fs = require('fs');
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

const blacklist = new Set(['index.js', '_validate.js']);
const dateRegex = /^\d{4}-\d{2}-\d{2}$/;

function generateMethodMap(dirname) {
	const files = fs.readdirSync(dirname) // eslint-disable-line no-sync
		.filter(f => (f.match(/\.js$/)) && !blacklist.has(f))
		.map(f => ({ file: f, method: require(`${dirname}/${f}`).default })); // eslint-disable-line global-require

	const methods = {};
	const versionMap = {};
	const allVersions = new Set();

	for (const { file, method } of files) {
		if (!method)
			throw log.warn('invalid_export', { file });

		const removedAt = checkDate(method.removedAt);
		const preview = method.preview || null;
		const versions = [];

		if (preview && typeof preview !== 'function')
			throw log.warn('invalid_export', { file, method });

		for (const version of Object.keys(method.versions)) {
			const date = checkDate(version);
			const impl = method.versions[version];

			if (!date || typeof impl !== 'object')
				throw log.warn('invalid_export', { file, method });

			if (!impl.validator || typeof impl.func !== 'function')
				throw log.warn('invalid_implementation', { file, method });

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
