import Browser from 'zombie';
import jsonClient from 'json-client';
import log from '@branch-app/log';
import querystring from 'querystring';

/* eslint-disable max-len, no-param-reassign, no-unused-expressions, no-invalid-this */

const authExpiry = 55 * 60;
const authUrl = 'https://login.live.com/oauth20_authorize.srf?client_id=0000000048093EE3&redirect_uri=https://login.live.com/oauth20_desktop.srf&response_type=token&display=touch&scope=service::user.auth.xboxlive.com::MBI_SSL';
const tokenName = 'access_token';
const userAgent = 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.104 Safari/537.36';
const redisKey = 'token:xbox-live';

export default async function getXboxLiveToken() {
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

	const url = browser.url;
	const index = url.indexOf(tokenName);

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

	const response = {
		token: xstsAuth.Token,
		...xstsAuth.DisplayClaims.xui[0],
		expiresAt: new Date(xstsAuth.NotAfter),
	};

	this.redis.set(redisKey, JSON.stringify(response), 'EX', authExpiry);

	return response;
}
