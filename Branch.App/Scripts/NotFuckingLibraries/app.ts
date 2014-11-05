///<reference path="../typings/jquery/jquery.d.ts" />
///<reference path="search-autocomplete.ts"/>

class Application {
	public xx: string = "yy";
	public autocomplete: SearchAutoComplete;

	constructor() {
		this.autocomplete = new SearchAutoComplete();
	}
}

// Load this shit
window.onload = () => {
	var app = new Application();
};
