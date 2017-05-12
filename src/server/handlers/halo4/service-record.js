import App from '../../../app';
import { IdentityType } from '../../../services/index';

const metadataScopes = ['achievements', 'difficulty', 'spartanOps'];

export default async function serviceRecord(app: App, req, res) {
	const paramIdentity = req.params.identity;

	try {
		const [
			identity,
			serviceRecord,
			metadata,
			metadataOptions,
		] = await Promise.all([
			app.xblClient.getIdentity(paramIdentity, IdentityType.Gamertag),
			app.halo4Client.getServiceRecord(paramIdentity, IdentityType.Gamertag),
			app.halo4Client.getMetadata(...metadataScopes),
			app.halo4Client.getMetadataOptions(),
		]);

		res.json({
			identity,
			serviceRecord,
			metadata,
			metadataOptions,
		});
	} catch (error) {
		throw error;
	}
}
