const path = require('path');

module.exports = function ({ paths }) {
	const { appSrc } = paths;

	return {
		webpack: {
			alias: {
				"@/home": path.join(appSrc, "app/home"),
				"@/lib": path.join(appSrc, "lib"),
			},
		},
	};
};
