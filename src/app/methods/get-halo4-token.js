import Browser from 'zombie';
import { calculateHash } from '../helpers/sha512';
import camelCase from 'camelcase-keys';
import log from '@branch-app/log';
import moment from 'moment';

/* eslint-disable max-len, no-param-reassign, no-unused-expressions */
const AuthUrl = 'https://login.live.com/oauth20_authorize.srf?client_id=000000004C0BD2F1&scope=xbox.basic+xbox.offline_access&response_type=code&redirect_uri=https://haloreachstats.halowaypoint.com/oauth/callback&state=https%253a%252f%252fapp.halowaypoint.com%252foauth%252fspartanToken&display=touch';
const TokenName = 'SpartanToken';
const UserAgent = 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.104 Safari/537.36';

export default async function getHalo4Token(ignoreCache: boolean, account: ?string, password: ?string) {
	account = account || this.config.halo4.account;
	password = password || this.config.halo4.password;

	const accountHash = calculateHash(`${account.toLowerCase()}:${password}`, this.config.hashingSalt);

	if (!ignoreCache) {
		try {
			const cachedToken = await this.db.halo4.findMostRecentToken(accountHash);

			if (cachedToken)
				return cachedToken;
		} catch (error) {
			if (error.code !== 'not_found')
				throw error;
		}
	}

	const browser = new Browser();

	browser.userAgent = UserAgent;
	await browser.visit(AuthUrl);
	await browser.fill('input[type=email]', account).pressButton('Next');
	await browser.fill('input[type=password]', password).pressButton('Sign in');

	const body = browser.text();
	const index = body.indexOf(TokenName);

	if (index < 0) throw log.error('token_parse_error', { body });

	const expiresAt = moment.utc().add(55, 'm');
	const parsedBody = camelCase(JSON.parse(body));

	const token = await this.db.halo4.createOne({
		accountHash,
		createdAt: new Date(),
		expiresAt: expiresAt.toDate(),
		...parsedBody,
	});

	return token;
}
