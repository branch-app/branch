import Collection from '../collection';
import { UUID } from '../id-providers';

type Type = {
	createdAt: Date,
	expiresAt: Date,

	accountHash: string,
	spartanToken: string,
	gamertag: string,
	analyticsToken: string,
};

export default class Halo4 extends Collection<Type> {
	static idProvider = UUID;

	async setupIndexes(): Promise<void> {
		await Promise.all([
			this.index(['createdAt', 'updatedAt', 'expiresAt', 'accountHash']),
		]);
	}

	async findMostRecentToken(accountHash: string): Promise<Type> {
		const sort = {
			expiresAt: -1,
		};
		const query = {
			accountHash,
			expiresAt: {
				$gte: new Date(Date.now() + (300 * 1000)),
			},
		};

		return await this.findOne(query, { sort });
	}
}
