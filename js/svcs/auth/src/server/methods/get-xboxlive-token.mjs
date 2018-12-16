import validator from './_validate';

export default {
	name: 'get_xboxlive_token',
	versions: {
		'2018-03-21': {
			func: getXboxLiveToken,
			validator: validator('get-xboxlive-token'),
		},
	},
};

async function getXboxLiveToken(ctx) {
	const { app, input } = ctx;

	return await app.getXboxLiveToken(input.forceRefresh === true);
}
