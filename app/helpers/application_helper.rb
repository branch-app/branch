module ApplicationHelper
	def gamertag_to_html(gamertag)
		return CGI.escape(gamertag)
	end

	def generate_facebook_share_url()
		return "https://www.facebook.com/sharer/sharer.php?u=#{CGI.escape(request.original_url())}"
	end

	def generate_twitter_share_url(text, via = 'alexerax', hashtag = 'branchapp', related = nil)
		url = "https://twitter.com/intent/tweet?original_referer=#{CGI.escape(request.original_url())}"
		url += "&related=#{related}" if related
		url += "&text=#{text}"
		url += "&tw_p=tweetbutton"
		url += "&url=#{CGI.escape(request.original_url())}"
		url += "&hashtags=#{hashtag}" if hashtag
		url += "&via=#{via}" if via

		return url
	end
end
