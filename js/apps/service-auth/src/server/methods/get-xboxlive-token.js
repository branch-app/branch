import validator from './_validate';

export default {
	name: 'get_xboxlive_token',
	versions: {
		'2018-03-21': getXboxLiveToken,
	},
};

async function getXboxLiveToken(ctx) {
	const { app, input } = ctx;

	validator('get-xboxlive-token')(input);

	return await app.getXboxLiveToken();
}
