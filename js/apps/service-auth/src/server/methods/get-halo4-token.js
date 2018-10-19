import validator from './_validate';

export default {
	name: 'get_halo4_token',
	versions: {
		'2018-03-21': {
			func: getHalo4Token,
			validator: validator('get-halo4-token'),
		},
	},
};

async function getHalo4Token(ctx) {
	const { app, input } = ctx;

	return await app.getHalo4Token(input.forceRefresh === true);
}
