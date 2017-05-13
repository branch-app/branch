import Browser from 'zombie';
import querystring from 'querystring';
import log from '@branch-app/log';
import moment from 'moment';

const TokenName = 'SpartanToken';

export default async function createHalo4Auth(account, password) {
	try {
		let browser = new Browser();
		browser.userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.104 Safari/537.36";
		await browser.visit("https://login.live.com/oauth20_authorize.srf" +
			"?client_id=000000004C0BD2F1" +
			"&scope=xbox.basic+xbox.offline_access" +
			"&response_type=code" +
			"&redirect_uri=https://haloreachstats.halowaypoint.com/oauth/callback&state=https%253a%252f%252fapp.halowaypoint.com%252foauth%252fspartanToken" +
			"&display=touch");

		await browser
			.fill("input[type=email]", account)
			.pressButton("Next");

		await browser
			.fill("input[type=password]", password)
			.pressButton("Sign in");

		const body = browser.text();
		const index = body.indexOf(TokenName);
		if (index < 0) {
			throw log.error('unable_to_retrieve_halo_4_tokens');
		}

		const data = JSON.parse(body);

		data.expiresAt = moment.utc().add(55, 'm').format();

		return data;
	} catch (error) {
		throw log.warn('unable_to_authenticate', [error]);
	}
}
