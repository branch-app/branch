// Place all the behaviors and hooks related to the matching controller here.
// All this logic will automatically be available in application.js.

$(document).ready(function() {
	$.getJSON('https://api.github.com/repos/xerax/branch/commits', function(data) {
		var commit = data[0];

		// Link to last commit
		$('.project-stats > li > #last-commit').attr('href', "https://github.com/Xerax/Branch/commit/" + commit.sha);
		$('.project-stats > li > #last-commit').text(commit.sha.substring(0,7));

		// Link to last commiter
		$('.project-stats > li > #last-commiter').attr('href', "https://github.com/" + commit.committer.login);
		$('.project-stats > li > #last-commiter').text(commit.committer.login);

		setTimeout(function() {
			$('.project-stats').fadeIn(600)
		}, 1000);
	});
});