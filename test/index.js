GLOBAL.projRequire = function (module) {
	return require('../' + module);
};

const log =  projRequire('lib');
const tc =   require('test-console');
const test = require('tape');
