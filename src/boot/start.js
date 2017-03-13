/* eslint-disable no-process-env, no-process-exit, func-style */

import 'babel-polyfill';
import App from '../app';
import Server from '../server';
import log from 'branch-log';
import logSentry from 'branch-log-sentry';
import raven from 'raven';

const defaultPort = 3000;
const port = process.env.PORT || defaultPort;
const config = JSON.parse(process.env.CONFIG);

const sentry = new raven.Client(config.sentryDSN);

log.setHandler(logSentry(sentry));
log.setHandler('fatal', logSentry(sentry, () => process.exit(1)));
sentry.patchGlobal(() => process.exit(1));

const run = async () => {
	const app = new App(config);
	const server = new Server(app, { port });

	await server.setup();
	server.run();
};

(async () => {
	try {
		await run();
	} catch (error) {
		log.fatal('start_failed', [error]);
	}
})();
