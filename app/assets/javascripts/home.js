$(jQuery).ready(function () {
	if (!$("#home-index").length)
		return;

	// Home Page slideshow
	const interval = 8000; // 8 seconds
	const element = $('.header');
	let imageIndex = 1;
	const images = [
		'/images/home/halo-5.jpg',
		'/images/home/destiny.jpg',
		'/images/home/halo-4.jpg',
		'/images/home/halo-reach.jpg',
		'/images/home/halo-3.jpg',
		'/images/home/halo-3-odst.jpg',
	];

	// Cache images
	images.forEach(function (img) {
		new Image().src = img;
	});

	// Slide through images in background
	setInterval(function () {
		if (imageIndex >= images.length) imageIndex = 0;
		const image = images[imageIndex];
		element.css("background-image", "url("+image+")");
		imageIndex++;
	}, interval);

	// Typed placeholder thang
	$(document).ready(function () {
		$('#player-ident').typed({
			strings: [
				'Major Nelson',
				'xyoatcakes',
				'Bravo',
				'P3',
				'iBot',
				'Program (rip)',
			],
			attr: 'placeholder',
			typeSpeed: 30,
			loop: true,
			startDelay: 0,
			backDelay: 1200,
		});
	});

	$('#player-search').submit(function (event) {
		// If the user doesn't have Javascript, it'll do a POST to /search/ and we can
		// handle it that way.
		event.preventDefault();
		var gamertag = $('#player-ident').val();
		window.location = '/xbox-live/' + gamertag + '/';
	});
});
