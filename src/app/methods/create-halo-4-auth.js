import Browser from 'zombie';
import querystring from 'querystring';
import log from 'cuvva-log';

const TokenName = 'SpartanToken';

export default async function createXboxLiveAuth(account, password) {
	let browser = new Browser();
	browser.userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.104 Safari/537.36";
	await browser.visit("https://app.halowaypoint.com/oauth/spartanToken");
		
	await browser
		.fill("input[type=email]", account)
		.pressButton("Next");

	await browser
		.fill("input[type=password]", password)
		.pressButton("Sign in");

	const body = browser.text;
	const index = body.indexOf(TokenName);
	if (index < 0) {
		throw log.error('unable_to_authenticate_with_halo_4')
	}

	return JSON.parse(body);
}
