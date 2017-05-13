/* eslint-disable no-process-env, no-process-exit, no-magic-numbers, func-style */

import 'babel-polyfill';
import App from '../app';
import Server from '../server';
import log from '@branch-app/log';
import { readFileSync } from 'fs';

// Import config
const defaultPort = 3000;
const config = JSON.parse(readFileSync('./config.development.json'));

const run = async () => {
	const options = {
		port: config.port || defaultPort,
	};
	const app = new App(config);
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
