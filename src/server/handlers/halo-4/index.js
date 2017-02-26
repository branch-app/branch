export async function index(ctx) {
	const { app, input } = ctx;
	const config = app.config;

	return await app.createHalo4Auth(config.halo4.account, config.halo4.password);
}
