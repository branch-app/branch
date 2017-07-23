export default function h4DifficultyAsset(level: ?number, size: string = 'large', options) {
	const root = options.data.root;
	const resolveAssetHelper = root.helpers.h4ResolveAsset;
	const difficultyMetadata = root.metadata.difficultiesMetadata.difficulties;

	if (!level) {
		return resolveAssetHelper({
			baseUrl: 'H4DifficultyAssets',
			assetUrl: '{size}/in-progress.png',
		}, size, options);
	}

	const difficulty = difficultyMetadata.find(d => d.id === level);
	const difficultyAsset = resolveAssetHelper(difficulty.imageUrl, size, options);

	return difficultyAsset;
}
