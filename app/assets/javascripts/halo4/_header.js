$(jQuery).ready(function () {
	if (!$("#halo4-service-record").length)
		return;

	// Recent Match Slideshow
	const interval = 6000; // 6 seconds
	const element = $('article > .header');
	let matchIndex = 0;
	let lastIndex = 0;
	let canUpdate = true;
	let intervalId = -1;

	// Cache images
	recentMatchImages.forEach(function (image) {
		new Image().src = image;
	});
	
	// Slide through matches
	function updateRecentMatch() {
		// Prevent animation collision
		canUpdate = false;

		// Inc match index, validate, and get match info
		matchIndex++;
		if (matchIndex >= recentMatches.length) matchIndex = 0;
		const recentMatch = recentMatches[matchIndex];

		// Remove active from last match indicator
		$('.item-container[data-index="' + lastIndex + '"] > div').removeClass('active');

		// Add active to this match indicator, and update background
		$('.item-container[data-index="' + matchIndex + '"] > div').addClass('active');
		element.css('background-image', "url('" + recentMatchImages[matchIndex] + "')");

		// Update text
		$('.match-overview > .match-mode').text(recentMatch.variantName);
		$('.match-overview > .match-map').text(recentMatch.mapVariantName);
		$('.personal-score > .value').text(recentMatch.personalScore);
		$('.medals > .value').text(recentMatch.totalMedals);
		$('.featured-stat > .value').text(recentMatch.featuredStatValue);
		$('.featured-stat > .key > .header-key').text(recentMatch.featuredStatName);
		$('.featured-stat > .key > .sub-key').text(recentMatch.variantName);

		// Set last match index
		lastIndex = matchIndex;

		// Prevent animation collision
		setTimeout(function () {
			canUpdate = true;
		}, 600);
	}
	intervalId = setInterval(updateRecentMatch, interval);

	// Enable clicking on recent matches
	$('.item-container').click(function () {
		const item = $(this);
		const index = item.data('index');
		lastIndex = matchIndex;
		matchIndex = index - 1;
		if (matchIndex < 0) matchIndex = recentMatches.length;

		// Clear the set interval, update the match, and restart the interval
		window.clearInterval(intervalId);
		updateRecentMatch();
		intervalId = setInterval(updateRecentMatch, interval);
	});
});
