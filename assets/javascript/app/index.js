import $ from 'jquery';

export default class App {
	options;

	constructor(options) {
		this.options = options;

		this.setupEvents();
		this.setup();
	}

	setup() {
		// Set copyright year
		$('span.copyright-year').text(new Date().getUTCFullYear());
	}

	setupEvents() {

	}
}
