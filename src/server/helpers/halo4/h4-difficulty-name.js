export default function h4DifficultyName(level: ?number, options) {
	const root = options.data.root;

	if (!level)
		return 'In progress';

	return root.metadata.difficultiesMetadata.difficulties.find(d => d.id === level).name;
}
