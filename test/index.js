GLOBAL.projRequire = function (module) {
	return require('../' + module);
};

const log = projRequire('lib');
const tc = require('test-console');
const test = require('tape');

log.setMinLogLevel('debug');

test('log.debug() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () {
		log.debug('test_error');
	});

	t.equals(err[0], 'debug:{"code":"test_error"}\n');
	t.end();
});

test('log.info() prints correctly', function (t) {
	var err = tc.stdout.inspectSync(function () {
		log.info('test_error');
	});

	t.equals(err[0], 'info:{"code":"test_error"}\n');
	t.end();
});

test('log.warn() prints correctly', function (t) {
	var err = tc.stderr.inspectSync(function () {
		log.warn('test_error');
	});

	t.equals(err[0], 'warn:{"code":"test_error"}\n');
	t.end();
});

test('log.error() prints correctly', function (t) {
	var err = tc.stderr.inspectSync(function () {
		log.error('test_error');
	});

	t.equals(err[0], 'error:{"code":"test_error"}\n');
	t.end();
});

test('log.fatal() prints correctly', function (t) {
	var err = tc.stderr.inspectSync(function () {
		log.fatal('test_error');
	});

	t.equals(err[0], 'fatal:{"code":"test_error"}\n');
	t.end();
});

test('log prints JSON correctly', function (t) {
	var err = tc.stderr.inspectSync(function () {
		log.error('test_error', { a: 'b', c: 'd' });
	});

	t.equals(err[0], 'error:{"code":"test_error","meta":{"a":"b","c":"d"}}\n');
	t.end();
});
