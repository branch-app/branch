import App from '../../../app';
import h4Helpers from '../../helpers/halo4';
import helpers from '../../helpers';

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

		const warGames = serviceRecord.gameModes.find(m => m.id === 3);
		const campaign = serviceRecord.gameModes.find(m => m.id === 4);
		const spartanOps = serviceRecord.gameModes.find(m => m.id === 5);
		const customGames = serviceRecord.gameModes.find(m => m.id === 6);

		let rankProgression;
		let rankProgressionComparison = ' - Max Rank Achieved';

		{
			const xp = serviceRecord.xp;
			const rankStartXp = serviceRecord.rankStartXp;
			const nextRankStartXp = serviceRecord.nextRankStartXp;

			rankProgression = helpers.calculatePercentage(xp - rankStartXp, nextRankStartXp - rankStartXp);

			if (serviceRecord.nextRankName)
				rankProgressionComparison = `(${helpers.numberWithDelim(xp)}/${helpers.numberWithDelim(nextRankStartXp)})`;
			else
				rankProgression = 100;
		}

		res.render('halo4/service-record/index', {
			title: `${identity.gamertag}'s Halo 4 Service Record - Branch`,
			sidebar: 'service-record',

			helpers: h4Helpers,
			identity,
			metadata,
			metadataOptions,
			serviceRecord,
			headerMatches,

			modeStats: {
				warGames,
				campaign,
				spartanOps,
				customGames,
			},
			totals: {
				kills: serviceRecord.gameModes.map(m => m.totalKills).reduce((a, b) => a + b, 0),
				deaths: serviceRecord.gameModes.map(m => m.totalDeaths).reduce((a, b) => a + b, 0),
				gamesStarted: serviceRecord.gameModes.map(m => m.totalGamesStarted).reduce((a, b) => a + b, 0),
			},
			percentages: {
				rankProgression,
				rankProgressionComparison,
				spartanOpsSpCompletion: helpers.calculatePercentage(spartanOps.totalSinglePlayerMissionsCompleted, spartanOps.totalMissionsPossible),
				spartanOpsCoopCompletion: helpers.calculatePercentage(spartanOps.totalCoopMissionsCompleted, spartanOps.totalMissionsPossible),
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
