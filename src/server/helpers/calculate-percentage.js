export default function calculatePercentage(a: number, b: number, places: ?number = 2) {
	const percentageStr = ((a / b) * 100).toString();

	return parseFloat(percentageStr).toFixed(places);
}
