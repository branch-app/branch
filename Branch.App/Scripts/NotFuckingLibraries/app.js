///<reference path="../typings/jquery/jquery.d.ts" />
///<reference path="search-autocomplete.ts"/>
var Application = (function () {
    function Application() {
        this.xx = "yy";
        this.autocomplete = new SearchAutoComplete();
    }
    return Application;
})();
// Load this shit
window.onload = function () {
    var app = new Application();
};
//# sourceMappingURL=app.js.map