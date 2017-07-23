export default function calculateKdRatio(kills, deaths) {
	if (deaths === 0)
		return kills;

	return Math.round(kills / deaths);
}
