GLOBAL.projRequire = function (module) {
	return require('../' + module);
};

const log = projRequire('lib');
const tc = require('test-console');
const test = require('tape');

test('log.debug() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () {
		log.debug('test-error');
	});

	t.assert(err[0].match(/^debug *: test-error/));
	t.end();
});

test('log.info() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () {
		log.info('test-error');
	});

	t.assert(err[0].match(/^info *: test-error/));
	t.end();
});

test('log.warn() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () {
		log.warn('test-error');
	});

	t.assert(err[0].match(/^warn *: test-error/));
	t.end();
});

test('log.error() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () {
		log.error('test-error');
	});

	t.assert(err[0].match(/^error *: test-error/));
	t.end();
});

test('log.fatal() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () {
		log.fatal('test-error');
	});

	t.assert(err[0].match(/^fatal *: test-error/));
	t.end();
});

test('log prints JSON correctly', function (t) {
	var err = tc.stdout.inspectSync(function () {
		log.error('test-error', { a: 'b', c: 'd' });
	});

	var matchreg = /^error *: test-error\s*{((\s*\"a\"\s*:\s*\"b\"\s*|\s*\"c\"\s*:\s*"d"\s*),?){2}}/;
	t.assert(err[0].match(matchreg));
	t.end();
});
