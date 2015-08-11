GLOBAL.projRequire = function (module) {
	return require('../' + module);
}

const log	 = projRequire('lib');
const tc     = require('test-console');
const test   = require('tape');

test('log.info() prints correctly', function (t) {
	let err = tc.stdout.inspectSync(() => log.info("test-error"));

	let matchreg = /^info *: test-error/;
	
	t.equal(err[0].search(matchreg), 0);
	t.end();
});

test('log.warn() prints correctly', function (t) {
	let err = tc.stdout.inspectSync(() => log.warn("test-error"));

	let matchreg = /^warn *: test-error/;
	
	t.equal(err[0].search(matchreg), 0);
	t.end();
});

test('log.error() prints correctly', function (t) {
	let err = tc.stdout.inspectSync(() => log.error("test-error"));

	let matchreg = /^error *: test-error/;
	
	t.equal(err[0].search(matchreg), 0);
	t.end();
});
