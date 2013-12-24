$.fn.flashMessage = function (options) {
	var target = this;
	options = $.extend({}, options);
	if (!options.message) {
		options.message = $.cookie("FlashMessage");
		$.cookie("FlashMessage", null, { path: '/' });
	}
	if (options.message) {
		var json = $.parseJSON(options.message);
		var alertClass = "alert-";

		switch (json[0]) {
			case "success":
			case "info":
			case "warning":
				alertClass += json[0];
				break;

			case "failure":
				alertClass += "danger";
				break;
		}

		$(this).addClass(alertClass);
		$('strong', this).text(json[1]);
		$('span', this).text(json[2]);
	}

	if (target.children().length === 0)
		return null;

	target.fadeIn().one("click", function () {
		$(this).fadeOut();
	});

	if (options.timeout > 0) {
		setTimeout(function () { target.fadeOut(); }, options.timeout);
	}

	return this;
};