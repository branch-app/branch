import log from '@branch-app/log';
import puppeteer from 'puppeteer';

/* eslint-disable max-len, no-param-reassign, no-unused-expressions, no-invalid-this */

const authExpiry = 55 * 60;
const authUrl = 'https://login.live.com/oauth20_authorize.srf?client_id=000000004C0BD2F1&scope=xbox.basic+xbox.offline_access&response_type=code&redirect_uri=https://haloreachstats.halowaypoint.com/oauth/callback&state=https%253a%252f%252fapp.halowaypoint.com%252foauth%252fspartanToken&display=touch';
const tokenName = 'SpartanToken';
const redisKey = 'token:halo-4';

export default async function getHalo4Token(forceRefresh) {
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

	const tokenStr = await page.evaluate(() => {
		const pre = document.querySelector('pre');

		return pre ? pre.textContent : null;
	});

	const resp = {
		...JSON.parse(tokenStr),
		expiresAt: new Date(Date.now() + (authExpiry * 1000)),
	};

	this.redis.set(redisKey, JSON.stringify(resp), 'EX', authExpiry);

	return resp;
}
