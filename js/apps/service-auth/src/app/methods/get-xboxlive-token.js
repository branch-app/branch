import Browser from 'zombie';
import { calculateHash } from '../helpers/sha512';
import jsonClient from 'json-client';
import log from '@branch-app/log';
import querystring from 'querystring';

/* eslint-disable max-len, no-param-reassign, no-unused-expressions */
const AuthUrl = 'https://login.live.com/oauth20_authorize.srf?client_id=0000000048093EE3&redirect_uri=https://login.live.com/oauth20_desktop.srf&response_type=token&display=touch&scope=service::user.auth.xboxlive.com::MBI_SSL';
const TokenName = 'access_token';
const UserAgent = 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.104 Safari/537.36';

export default async function getXboxLiveToken(ignoreCache: boolean, account: ?string, password: ?string) {
	account = account || this.config.xboxlive.account;
	password = password || this.config.xboxlive.password;

	const accountHash = calculateHash(`${account.toLowerCase()}:${password}`, this.config.hashingSalt);

	if (!ignoreCache) {
		try {
			const cachedToken = await this.db.xboxlive.findMostRecentToken(accountHash);

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

	const url = browser.url;
	const index = url.indexOf(TokenName);

	if (index < 0) throw log.error('unable_to_retrieve_tokens', [], { url });

	const parsedBody = querystring.parse(url.substring(index));
	const usrAuthClient = jsonClient('https://user.auth.xboxlive.com');
	const xstsClient = jsonClient('https://xsts.auth.xboxlive.com');

	const usrAuth = await usrAuthClient('post', 'user/authenticate', null, {
		Properties: {
			AuthMethod: 'RPS',
			RpsTicket: `t=${parsedBody.access_token}`,
			SiteName: 'user.auth.xboxlive.com',
		},
		RelyingParty: 'http://auth.xboxlive.com',
		TokenType: 'JWT',
	});
	const xstsAuth = await xstsClient('post', 'xsts/authorize', null, {
		Properties: {
			SandboxId: 'RETAIL',
			UserTokens: [usrAuth.Token],
		},
		RelyingParty: 'http://xboxlive.com',
		TokenType: 'JWT',
	});

	const token = await this.db.xboxlive.createOne({
		accountHash,
		createdAt: new Date(),
		expiresAt: new Date(xstsAuth.NotAfter),
		token: xstsAuth.Token,
		...xstsAuth.DisplayClaims.xui[0],
	});

	return token;
}
