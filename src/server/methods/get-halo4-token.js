import validator from './_validate';

export default async function (ctx) {
	const { app, input } = ctx;

	validator('get-halo4-token')(input);

	return await app.getHalo4Token(input.ignore_cache || false, input.account, input.password);
}
