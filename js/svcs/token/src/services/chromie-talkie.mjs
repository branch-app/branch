import { delay } from '../helpers/promise';
import jsonClient from 'json-client';
import puppeteer from 'puppeteer';

export default class ChromieTalkie {
	constructor(remoteChrome) {
		this.options = { remoteChrome };

		if (!this.options.remoteChrome)
			return;

		this.chromeClient = jsonClient(`http://${this.options.remoteChrome}`, {
			headers: {
				// Workaround needed when running docker, so chrome thinks the request is
				// coming from...inside the house
				host: '0.0.0.0',
			},
		});
	}

	async getBrowser() {
		if (!this.options.remoteChrome)
			return await puppeteer.launch();

		const meta = await this.chromeClient('get', '/json/version');
		const url = this._generateUrl(meta.webSocketDebuggerUrl);

		return await puppeteer.connect({
			browserWSEndpoint: url,
		});
	}

	async typeAndSubmit(page, content) {
		await page.keyboard.type(content);
		await Promise.all([
			delay(1000),
			page.click('input[type=submit]'),
		]);
	}

	_generateUrl(webSocketDebuggerUrl) {
		const index = webSocketDebuggerUrl.lastIndexOf('/');
		const id = webSocketDebuggerUrl.slice(index + 1, webSocketDebuggerUrl.length);

		return `ws://${this.options.remoteChrome}/devtools/browser/${id}`;
	}
}
