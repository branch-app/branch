import $ from 'jquery';
import 'typedjs'; // eslint-disable-line sort-imports

const interval = 8000;

export default class Home {
	headerElement: HTMLElement;
	imageIndex: number = 1;
	images: string[] = [
		'/public/images/home/halo-5.jpg',
		'/public/images/home/destiny.jpg',
		'/public/images/home/halo-4.jpg',
		'/public/images/home/halo-reach.jpg',
		'/public/images/home/halo-3.jpg',
		'/public/images/home/halo-3-odst.jpg',
	];

	setup() {
		if (!$('#home-index').length) return;

		this._setupEvents();
		this._cacheImages();
		this._initSlideshow();
		this._initTypedPlaceholder();
	}

	_setupEvents() {
		$('#player-search').submit(this._lookupPlayer);
	}

	_cacheImages() {
		this.images.forEach(img => {
			new Image().src = img;
		});
	}

	_initSlideshow() {
		this.headerElement = document.getElementsByClassName('header')[0];

		window.setInterval((() => {
			if (this.imageIndex >= this.images.length) this.imageIndex = 0;
			const image = this.images[this.imageIndex];

			this.headerElement.style.backgroundImage = `url(${image})`;
			this.imageIndex += 1;
		}), interval);
	}

	_initTypedPlaceholder() {
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
	}

	_lookupPlayer(event) {
		event.preventDefault();
		const gamertag = $('#player-ident').val();

		window.location = `/xbox-live/${gamertag}/`;
	}
}
