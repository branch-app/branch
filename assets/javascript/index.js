import $ from 'jquery';
import Home from './home';
import 'bootstrap'; // eslint-disable-line sort-imports

const navbarStateChangeIndex = 55;

type Options = {

};

export default class App {
	options: Options;
	above: ?boolean = void 0;

	constructor(options: Options) {
		this.options = options;

		this._updateNavStyle();
	}

	setup() {
		this._setupView();
		this._setupEvents();
		this._setupSubApps();
	}

	_setupView() {
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

	_setupEvents() {
		$(window).scroll(this._updateNavStyle);
	}

	_setupSubApps() {
		const home = new Home();

		home.setup();
	}

	_updateNavStyle() {
		const prevValue = this.above;

		this.above = $(window).scrollTop() < navbarStateChangeIndex;
		if (prevValue === this.above) return;
		$('nav.navbar')[`${this.above ? 'remove' : 'add'}Class`]('navbar-secondary');
	}
}

const options = { };
const app = new App(options);

app.setup();
