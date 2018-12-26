/* eslint-disable no-process-env, no-process-exit, func-style, no-console */

import App from '../app';
import { ChromieTalkie } from '../services';
import RedisClient from '@branch-app/redis-client';
import Server from '../server/index';
import camelize from 'camelize';
import log from '@branch-app/log';
import { readFileSync } from 'fs';

// Import config
const defaultPort = 3000;
const config = camelize(JSON.parse(readFileSync('./config.development.json')));

const run = async () => {
	const options = { port: defaultPort };

	const talkie = new ChromieTalkie(config.remoteChromeHost);
	const redis = await RedisClient.connect(config.redis);
	const app = new App(config, talkie, redis);
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
