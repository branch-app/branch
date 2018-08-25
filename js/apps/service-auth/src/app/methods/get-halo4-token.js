import Browser from 'zombie';
import log from '@branch-app/log';

/* eslint-disable max-len, no-param-reassign, no-unused-expressions, no-invalid-this */

const authExpiry = 55 * 60;
const authUrl = 'https://login.live.com/oauth20_authorize.srf?client_id=000000004C0BD2F1&scope=xbox.basic+xbox.offline_access&response_type=code&redirect_uri=https://haloreachstats.halowaypoint.com/oauth/callback&state=https%253a%252f%252fapp.halowaypoint.com%252foauth%252fspartanToken&display=touch';
const tokenName = 'SpartanToken';
const userAgent = 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.104 Safari/537.36';
const redisKey = 'token:halo-4';

export default async function getHalo4Token() {
	try {
		const cachedToken = await this.redis.get(redisKey);

		if (cachedToken)
			return JSON.parse(cachedToken);
	} catch (error) {
		throw error;
	}

	const { microsoftAccount } = this.config.providers;
	const browser = new Browser();

	browser.userAgent = userAgent;
	await browser.visit(authUrl);
	await browser.fill('input[type=email]', microsoftAccount.account).pressButton('Next');
	await browser.fill('input[type=password]', microsoftAccount.password).pressButton('Sign in');

	const body = browser.text();
	const index = body.indexOf(tokenName);

	if (index < 0)
		throw log.error('token_parse_error', { body });

	const response = {
		...JSON.parse(body),
		expiresAt: new Date(Date.now() + (authExpiry * 1000)),
	};

	this.redis.set(redisKey, JSON.stringify(response), 'EX', authExpiry);

	return response;
}
