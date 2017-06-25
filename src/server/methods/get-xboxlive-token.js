import validator from './_validate';

export default async function (ctx) {
	const { app, input } = ctx;

	validator('get-xboxlive-token')(input);

	return await app.getXboxLiveToken(input.ignore_cache || false, input.account, input.password);
}
