import fs from 'fs';
import path from 'path';

export default function h4ResolveAsset(asset: {}, size, options) {
	const baseUrl = asset.baseUrl;
	const assetUrl = asset.assetUrl.replace('{size}', size.toString());

	const url = options.data.root.metadataOptions.settings[baseUrl];
	let publicPath = `/public/images/games/halo4/${baseUrl}/${assetUrl}`;

	if (baseUrl === 'H4MapAssets')
		publicPath = publicPath.replace('.png', '.jpg');

	if (fs.existsSync(path.join(__dirname, '../../../../', publicPath)))
		return publicPath;

	return url ? url + assetUrl : '';
}
