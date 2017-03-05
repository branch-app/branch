$(jQuery).ready(function() {
	// Set copyright year
	$('span.copyright-year').text(new Date().getUTCFullYear());

	// Listen for navbar scroll changes and update navbar class
	$(window).scroll(function () {
		const y = $(this).scrollTop();
		if (y < 100)
			$('nav.navbar').removeClass('navbar-secondary');
		else
			$('nav.navbar').addClass('navbar-secondary');
	});
})
