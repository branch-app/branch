/* eslint-disable no-process-env, no-process-exit, func-style */

import App from './app';
import log from 'cuvva-log';
import raven from 'raven';

// TODO: update cuvva-log-sentry to support client side sentry.
// const sentry = new raven.Client('https://7525a243339948ce84984112d5036cd7@sentry.io/167751');

log.setMinLogLevel('debug');
log.setHandler('fatal', () => process.exit(1));
// sentry.patchGlobal(() => process.exit(1));

const run = () => {
	const options = { };
	const app = new App(options);

	app.setup();
};

try {
	run();
} catch (error) {
	log.fatal('start_failed', [error], { stack: error.stack });
}
