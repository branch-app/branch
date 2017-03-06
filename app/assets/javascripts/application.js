// This is a manifest file that'll be compiled into application.js, which will include all the files
// listed below.
//
// Any JavaScript/Coffee file within this directory, lib/assets/javascripts, vendor/assets/javascripts,
// or any plugin's vendor/assets/javascripts directory can be referenced here using a relative path.
//
// It's not advisable to add code directly here, but if you do, it'll appear at the bottom of the
// compiled file. JavaScript code in this file should be added after the last require_* statement.
//
//= require turbolinks
//= require_tree .

$(jQuery).ready(function () {
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
});
