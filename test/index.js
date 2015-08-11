GLOBAL.projRequire = function (module) {
	return require('../' + module);
}

const log	 = projRequire('lib');
const tc     = require('test-console');
const test   = require('tape');

test('log.info() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () { log.info("test-error") });
	console.log(err[0]);
	var matchreg = /^info *: test-error/;
	
	t.equal(err[0].search(matchreg), 0);
	t.end();
});

test('log.warn() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () { log.warn("test-error") });
	console.log(err[0]);
	var matchreg = /^warn *: test-error/;
	
	t.equal(err[0].search(matchreg), 0);
	t.end();
});

test('log.error() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () { log.error("test-error") });
	console.log(err[0]);
	var matchreg = /^error *: test-error/;
	
	t.equal(err[0].search(matchreg), 0);
	t.end();
});

test('log prints JSON correctly', function (t) {
	var err = tc.stdout.inspectSync(function () { log.error("test-error", { a: "b", c: "d" }); });
	console.log(err[0]);
	var matchreg = /^error *: test-error\s*{((\s*\"a\"\s*:\s*\"b\"\s*|\s*\"c\"\s*:\s*"d"\s*),?){2}}/;
	
	t.equal(err[0].search(matchreg), 0);
	t.end();
});
