import Browser from 'zombie';
import querystring from 'querystring';
import log from 'branch-log';

const TokenName = 'access_token';

export default async function createXboxLiveAuth(account, password) {
	let browser = new Browser();
	browser.userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.104 Safari/537.36";
	await browser.visit("https://login.live.com/oauth20_authorize.srf?client_id=0000000048093EE3&redirect_uri=https://login.live.com/oauth20_desktop.srf&response_type=token&display=touch&scope=service::user.auth.xboxlive.com::MBI_SSL");
		
	await browser
		.fill("input[type=email]", account)
		.pressButton("Next");

	await browser
		.fill("input[type=password]", password)
		.pressButton("Sign in");

	const url = browser.url;
	const index = url.indexOf(TokenName);
	if (index < 0) {
		throw log.error('unable_to_authenticate_with_xbox_live')
	}

	const data = url.substring(index);
	return querystring.parse(data);
}
