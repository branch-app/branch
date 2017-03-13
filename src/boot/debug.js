/* eslint-disable no-process-env, no-process-exit, no-magic-numbers, func-style */

import 'babel-polyfill';
import App from '../app';
import Server from '../server';
import log from 'branch-log';
import fs from 'fs';

// Import config
let config = {};
if (fs.existsSync("./config.json")) {
	var data = fs.readFileSync("./config.json");
	config = JSON.parse(data);
} else {
	config = JSON.parse(process.env.CONFIG);
}

const defaultPort = 3000;
const port = config.port || defaultPort;

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
