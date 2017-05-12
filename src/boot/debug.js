/* eslint-disable no-process-env, no-process-exit, func-style */

import 'babel-polyfill';
import App from '../app';
import Server from '../server';
import log from 'cuvva-log';
import { readFileSync } from 'fs';
import * as Services from '../services';

const defaultPort = 3000;
const config = JSON.parse(readFileSync('./config.development.json'));

log.setMinLogLevel('debug');
log.setHandler('fatal', () => process.exit(1));

const run = async () => {
	const options = {
		port: config.port || defaultPort,
	};

	const app = new App(
		new Services.Halo4Client(config.halo4Url, config.halo4Key),
		new Services.XboxLiveClient(config.xboxliveUrl, config.xboxliveKey),
	);
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
