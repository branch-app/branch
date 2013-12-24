$.fn.flashMessage = function (options) {
	
	options = $.extend({}, options);
	if (!options.message) {
		options.message = $.cookie("FlashMessage");
		$.cookie("FlashMessage", null, { path: '/' });
	}

	var json;
	try { json = $.parseJSON(options.message); }
	catch (err) { return null; }

	if (json == null) return null;
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

	$(this).parent().css('display', 'block');
	$(this).addClass(alertClass);
	$('strong', this).text(json[1]);
	$('span', this).text(json[2]);

	return this;
};