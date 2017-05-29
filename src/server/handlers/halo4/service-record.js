import App from '../../../app';
import helpers from '../../helpers/halo4';

export default async function serviceRecord(app: App, req, res) {
	const paramIdentity = req.params.identity;

	try {
		const {
			identity,
			serviceRecord,
			headerMatches,
			metadata,
			metadataOptions,
		} = await app.Halo4.getServiceRecord(paramIdentity);

		serviceRecord.totalCommendationProgress *= 100;

		res.render('halo4/service-record/index', {
			title: `${identity.gamertag}'s Halo 4 Service Record - Branch`,
			sidebar: 'service-record',

			helpers,
			identity,
			metadata,
			metadataOptions,
			serviceRecord,
			headerMatches,

			modeStats: {
				warGames: serviceRecord.gameModes.find(m => m.id === 3),
				campaign: serviceRecord.gameModes.find(m => m.id === 4),
				spartanOps: serviceRecord.gameModes.find(m => m.id === 5),
				customGames: serviceRecord.gameModes.find(m => m.id === 6),
			},
			totals: {
				kills: serviceRecord.gameModes.map(m => m.totalKills).reduce((a, b) => a + b, 0),
				deaths: serviceRecord.gameModes.map(m => m.totalDeaths).reduce((a, b) => a + b, 0),
				gamesStarted: serviceRecord.gameModes.map(m => m.totalGamesStarted).reduce((a, b) => a + b, 0),
			},
			hasHeaderMatches: headerMatches.games.length > 0,
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
