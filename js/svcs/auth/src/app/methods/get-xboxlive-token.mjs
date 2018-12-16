import jsonClient from 'json-client';
import log from '@branch-app/log';
import puppeteer from 'puppeteer';
import querystring from 'querystring';

/* eslint-disable max-len, no-param-reassign, no-unused-expressions, no-invalid-this */

const usrAuthClient = jsonClient('https://user.auth.xboxlive.com');
const xstsClient = jsonClient('https://xsts.auth.xboxlive.com');

const authExpiry = 55 * 60;
const authUrl = 'https://login.live.com/oauth20_authorize.srf?client_id=0000000048093EE3&redirect_uri=https://login.live.com/oauth20_desktop.srf&response_type=token&display=touch&scope=service::user.auth.xboxlive.com::MBI_SSL';
const tokenName = 'access_token';
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

	const { microsoftAccount } = this.config.providers;
	const pi = await puppeteer.launch();
	const page = await pi.newPage();

	await page.goto(authUrl);

	// Set email
	await page.keyboard.type(microsoftAccount.account);
	await Promise.all([
		page.waitForNavigation({ waitUntil: 'networkidle2' }),
		page.click('input[type=submit]'),
	]);

	// Set password
	await page.keyboard.type(microsoftAccount.password);
	await Promise.all([
		page.waitForNavigation({ waitUntil: 'networkidle2' }),
		page.click('input[type=submit]'),
	]);

	const title = await page.evaluate(() => {
		const t = document.querySelector('#iPageTitle');

		return t ? t.textContent : null;
	});

	// There has been a terms update
	if (title && title.includes('terms')) {
		await Promise.all([
			page.waitForNavigation({ waitUntil: 'networkidle2' }),
			page.click('input[type=submit]'),
		]);
	}

	const url = await page.evaluate(() => window.location.hash);
	const parsedBody = querystring.parse(url.substring(1));

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
