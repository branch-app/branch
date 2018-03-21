/* eslint-disable no-process-env, no-process-exit, func-style */

import 'babel-polyfill';
import App from '../app';
import RedisClient from '@branch-app/redis-client';
import Server from '../server';
import camelize from 'camelize';
import log from '@branch-app/log';
import logSentry from '@branch-app/log-sentry';
import raven from 'raven';

const defaultPort = 3000;
const port = process.env.PORT || defaultPort;
const config = camelize(JSON.parse(process.env.CONFIG));

const sentry = new raven.Client(config.sentryDSN);

log.setHandler(logSentry(sentry));
log.setHandler('fatal', logSentry(sentry, () => process.exit(1)));
sentry.patchGlobal(() => process.exit(1));

const run = async () => {
	const options = { port };

	const redis = await RedisClient.connect(config.redis);
	const app = new App(config, redis);
	const server = new Server(app, options);

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
