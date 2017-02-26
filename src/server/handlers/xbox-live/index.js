export async function index(ctx) {
	const { app, input } = ctx;
	const config = app.config;

	return await app.createXboxLiveAuth(config.xboxLive.account, config.xboxLive.password);
}
