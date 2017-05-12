import $ from 'jquery';

type Options = {

};

export default class App {
	options: Options;
	above: ?boolean = void 0;
	navbarStateChangeIndex: number = 55;

	constructor(options: Options) {
		this.options = options;

		this.setupView();
		this.setupEvents();
		this.updateNavStyle();
	}

	setupView() {
		// Set copyright year
		$('span.copyright-year').text(new Date().getUTCFullYear());

		// Enable popover
		$('[data-toggle=popover]').popover({
			html: true,
			content: () => {
				const content = $(this).attr('data-popover-content');

				return $(content)
					.children('.popover-body')
					.html();
			},
		});

		// Enable tool-tips
		$('[data-toggle="tooltip"]').tooltip();
	}

	setupEvents() {
		$(window).scroll(this.updateNavStyle);
	}

	updateNavStyle() {
		const prevValue = this.above;

		this.above = $(window).scrollTop() < this.navbarStateChangeIndex;
		if (prevValue === this.above) return;
		$('nav.navbar')[`${this.above ? 'remove' : 'add'}Class`]('navbar-secondary');
	}
}
