import App from '../../../app';

export default async function results(app: App, req, res) {
	const paramIdentity = req.params.identity;

	try {
		const {
			identity,
			h4PlayerCard,
		} = await app.Search.showSearchResults(paramIdentity);

		res.render('search/index', {
			title: `${identity.gamertag}'s Search Results - Branch`,

			identity,
			h4PlayerCard,
		});
	} catch (error) {
		switch (error.code) {
			case 'invalid_identity':
			case 'identity_doesnt_exist':
				throw Error('gamertag_doesnt_exist');

			case 'waypoint_no_data':
				throw Error('player_not_played_h4');

			default:
				throw error;
		}
	}
}
