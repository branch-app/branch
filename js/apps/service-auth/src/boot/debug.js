/* eslint-disable no-process-env, no-process-exit, func-style, no-console */

import 'babel-polyfill';
import App from '../app';
import RedisClient from 'redis-client';
import Server from '../server';
import camelize from 'camelize';
import log from 'branch-log';
import { readFileSync } from 'fs';

// Import config
const defaultPort = 3000;
const config = camelize(JSON.parse(readFileSync('./config.development.json')));

const run = async () => {
	const options = { port: defaultPort };

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
		console.log(error.stack);

		log.fatal('start_failed', [error]);
	}
})();
