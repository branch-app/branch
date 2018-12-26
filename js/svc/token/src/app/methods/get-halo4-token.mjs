/* eslint-disable max-len, no-param-reassign, no-unused-expressions, no-invalid-this */
import log from '@branch-app/log';

const authExpiry = 55 * 60;
const authUrl = 'https://login.live.com/oauth20_authorize.srf?client_id=000000004C0BD2F1&scope=xbox.basic+xbox.offline_access&response_type=code&redirect_uri=https://haloreachstats.halowaypoint.com/oauth/callback&state=https%253a%252f%252fapp.halowaypoint.com%252foauth%252fspartanToken&display=touch';
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

	const { microsoftAccount: { account, password } } = this.config.providers;
	const browser = await this.chromieTalkie.getBrowser();
	const page = await browser.newPage();

	try {
		await page.goto(authUrl);

		let tokenStr = await readPre(page);

		if (!tokenStr) {
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

		tokenStr = await readPre(page);

		if (!tokenStr)
			throw log.info('token_read_fail');

		const resp = {
			...JSON.parse(tokenStr),
			expiresAt: new Date(Date.now() + (authExpiry * 1000)),
		};

		this.redis.set(redisKey, JSON.stringify(resp), 'EX', authExpiry);

		return resp;
	} finally {
		await page.close();
	}
}

async function readPre(page) {
	return await page.evaluate(() => {
		const pre = window.document.querySelector('pre');

		return pre ? pre.textContent : null;
	});
}
