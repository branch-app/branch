import jsonClient from 'json-client';
import log from '@branch-app/log';
import querystring from 'querystring';

/* eslint-disable max-len, no-param-reassign, no-unused-expressions, no-invalid-this */

const usrAuthClient = jsonClient('https://user.auth.xboxlive.com');
const xstsClient = jsonClient('https://xsts.auth.xboxlive.com');

const authExpiry = 55 * 60;
const authUrl = 'https://login.live.com/oauth20_authorize.srf?client_id=0000000048093EE3&redirect_uri=https://login.live.com/oauth20_desktop.srf&response_type=token&display=touch&scope=service::user.auth.xboxlive.com::MBI_SSL';
const redisKey = 'token:xbox-live';

export default async function getXboxLiveToken(forceRefresh) {
	if (!forceRefresh) {
		try {
			const cachedToken = await this.redis.get(redisKey);

			if (cachedToken)
				return JSON.parse(cachedToken);
		} catch (error) {
			throw error;
		}
	}

	const { microsoftAccount: { account, password } } = this.config.providers;
	const browser = await this.chromieTalkie.getBrowser();
	const page = await browser.newPage();

	try {
		await page.goto(authUrl);

		let hash = await page.evaluate(() => window.location.hash);

		if (!hash) {
			// Set email
			await this.chromieTalkie.typeAndSubmit(page, account);
			await this.chromieTalkie.typeAndSubmit(page, password);

			const title = await page.evaluate(() => {
				const t = window.document.querySelector('#iPageTitle');

				return t ? t.textContent : null;
			});

			// There has been a terms update
			if (title && title.includes('terms')) {
				await Promise.all([
					page.waitForNavigation({ waitUntil: 'networkidle2' }),
					page.click('input[type=submit]'),
				]);
			}
		}

		hash = await page.evaluate(() => window.location.hash);

		if (!hash)
			throw log.info('token_read_fail');

		const parsedBody = querystring.parse(hash.substring(1));
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
	} finally {
		await page.close();
	}
}
