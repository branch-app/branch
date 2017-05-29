import humanizeDuration from 'humanize-duration';

export default function h4FriendlyDuration(duration: string, incSeconds: bool) {
	const regex = /(?:(\d{1,3}?)\.)?(\d{1,2}):(\d{1,2}):(\d{1,2})/i;
	const matches = regex.exec(duration);
	let totalMs = 0;

	matches.reverse().forEach((m, i) => {
		if (i === 0)
			totalMs += parseInt(m, 10) * 1000; // Seconds
		else if (i === 1)
			totalMs += parseInt(m, 10) * 60000; // Minutes
		else if (i === 2)
			totalMs += parseInt(m, 10) * 3600000; // Hours
		else if (i === 3)
			totalMs += parseInt(m, 10) * 86400000; // Days
	});

	if (incSeconds)
		return humanizeDuration(Math.round(totalMs / 1000) * 1000, { units: ['d', 'h', 'm', 's'] });

	return humanizeDuration(Math.round(totalMs / 60000) * 60000, { units: ['d', 'h', 'm'] });
}
