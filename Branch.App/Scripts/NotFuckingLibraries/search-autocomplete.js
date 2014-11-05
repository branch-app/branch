///<reference path="../typings/jquery/jquery.d.ts" />
var SearchAutoComplete = (function () {
    function SearchAutoComplete() {
        var _this = this;
        this.lastSearchTerm = "";
        this.timer = null;
        this.resultCount = 0;
        document.getElementById("search-input").onkeydown = function (event) {
            clearTimeout(_this.timer);
            _this.timer = null;

            console.log("[Search] key down", event.keyCode);

            if (((event.which) ? event.which : event.keyCode) == 27) {
                $("#search-input").val('');
                $(".auto-complete-container").fadeTo(250, 0, function () {
                    $(".auto-complete-container").css('display', 'none');
                });
            } else {
                _this.timer = setTimeout(_this.querySearchTerm, 100);
            }
        };

        document.onmousedown = function (event) {
            var container = $(".auto-complete-container");
            var input = $("#search-input");

            if ((!container.is(event.target)) && (!input.is(event.target))) {
                // Clear timer
                clearTimeout(_this.timer);
                _this.timer = null;

                // Hide UI
                container.fadeTo(250, 0, function () {
                    container.css('display', 'none');
                });
            }
        };

        document.getElementById("search-input").onmousedown = function () {
            if ($.trim(_this.lastSearchTerm) != "") {
                $(".auto-complete-container").fadeTo(250, 1);
            }
        };
    }
    SearchAutoComplete.prototype.querySearchTerm = function () {
        var _this = this;
        clearTimeout(this.timer);
        this.timer = null;

        console.log("[Search] Getting Query");

        var searchTerm = $.trim(document.getElementById("search-input").value);
        if (searchTerm == this.lastSearchTerm)
            return;
        this.lastSearchTerm = searchTerm;

        if (searchTerm == "") {
            // hide UI
            $(".auto-complete-container").fadeTo(250, 1, function () {
                $(".auto-complete-container").css('display', 'none');
            });

            // gtfo
            return;
        }

        console.log("[Search Term] Query is `" + searchTerm + "`");

        var innerHtml = "<ul id='ambigious-search-item' class='search-list'>" + "<li>" + "<a href='/Search/?q=" + searchTerm + "'>" + searchTerm + "</a>" + "</li>" + "</ul>" + "<div id='game-specific-items'></div>";
        $('.auto-complete-container').html(innerHtml);
        $('.auto-complete-container').fadeTo(250, 1);
        $('.auto-complete-container').css('display', 'block');

        $.ajax({
            url: "/Api/Search/Identities?id=" + searchTerm,
            accepts: "application/json"
        }).done(function (data) {
            console.info(JSON.stringify(data));

            _this.resultCount = 0;
            var games = [];
            for (var i = 0; i < data.length; i++) {
                if ($.inArray(data[i]["Ident"], games) == -1)
                    games.push(data[i]["Ident"]);
            }

            innerHtml = "";
            for (i = 0; i < games.length; i++) {
                var players = [];
                for (var j = 0; j < data.length; j++) {
                    if (data[j]["Ident"] == games[i]) {
                        players.push(data[j]);
                    }
                }

                innerHtml += "<hr class='search-divider' />" + "<h4 class='search-header'>" + games[i] + "</h4>" + "<ul class='search-list'>";

                for (var k = 0; k < players.length; k++) {
                    _this.resultCount++;
                    innerHtml += "<li>" + "<a href='" + players[k]["Url"] + "'>" + players[k]["GamerId"] + "</a>" + "<div class='search-sub-ident'>" + players[k]["ServiceTag"] + "</div>" + "</li>";
                }

                innerHtml += "</ul>";
            }
            $('#game-specific-items').html(innerHtml);
        });
    };
    return SearchAutoComplete;
})();
//	function querySearchTerm() {
//		clearInterval(timer);
//		timer = null;
//		console.log("doing ajax");
//		var searchTerm = $.trim(document.getElementById("search-input").value);
//		if (searchTerm == lastSearchTerm) return;
//		lastSearchTerm = searchTerm;
//		if (searchTerm == "") {
//			// hide UI
//			$(".auto-complete-container").fadeTo(250, 0, function () { $(".auto-complete-container").css('display', 'none'); });
//			// gtfo
//			return;
//		}
//		console.log("[Search Term] :: " + searchTerm);
//		//if (screen.width < 1200) return;
//		$.ajax({
//			url: "/Api/Search/Identities?id=" + searchTerm,
//			accepts: "application/json"
//		}).done(function (data) {
//				console.info(JSON.stringify(data));
//				var games = [];
//				for (var i = 0; i < data.length; i++) {
//					if ($.inArray(data[i]["Ident"], games) == -1)
//						games.push(data[i]["Ident"]);
//				}
//				var innerHtml =
//					"<ul class='search-list'>" +
//					"<li>" +
//					"<a href='/Search/?q=" + searchTerm + "'>" + searchTerm + "</a>" +
//					"</li>" +
//					"</ul>";
//				for (i = 0; i < games.length; i++) {
//					var players = [];
//					for (var j = 0; j < data.length; j++) {
//						if (data[j]["Ident"] == games[i]) {
//							players.push(data[j]);
//						}
//					}
//					innerHtml +=
//					"<hr class='search-divider' />" +
//					"<h4 class='search-header'>" + games[i] + "</h4>" +
//					"<ul class='search-list'>";
//					for (var k = 0; k < players.length; k++) {
//						innerHtml +=
//						"<li>" +
//						"<a href='" + players[k]["Url"] + "'>" +
//						players[k]["GamerId"] +
//						"</a>" +
//						"<div class='search-sub-ident'>" + players[k]["ServiceTag"] + "</div>" +
//						"</li>";
//					}
//					innerHtml += "</ul>";
//				}
//				$('.auto-complete-container').html(innerHtml);
//				$('.auto-complete-container').css('display', 'block');
//				$('.auto-complete-container').fadeTo(250, 1);
//			});
//	}
//}
//window.onload = function () {
//	var timer = null;
//	var lastSearchTerm = "";
//	document.getElementById("search-input").onkeydown = function () {
//		clearInterval(timer);
//		timer = null;
//		timer = setInterval(querySearchTerm, 100);
//	}
//	document.onkeydown = function (event) {
//		if (((event.which) ? event.which : event.keyCode) == 27) {
//			clearInterval(timer);
//			timer = null;
//			$("#search-input").val('');
//			$(".auto-complete-container").fadeTo(250, 0, function () { $(".auto-complete-container").css('display', 'none'); });
//		}
//	};
//	$(document).mouseup(function (e) {
//		var container = $(".auto-complete-container");
//		var input = $("#search-input");
//		if ((!container.is(e.target) && container.has(e.target).length === 0) && // not container
//			(!input.is(e.target) && input.has(e.target).length === 0)) // not input
//		{
//			container.fadeTo(250, 0, function () { container.css('display', 'none'); });
//		}
//	});
//	$('#search-input').mouseup(function () {
//		if (lastSearchTerm != "") {
//			$(".auto-complete-container").fadeTo(250, 1);
//		}
//	});
//# sourceMappingURL=search-autocomplete.js.map
