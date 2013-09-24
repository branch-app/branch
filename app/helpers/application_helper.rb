module ApplicationHelper
	def gamertag_to_html(gamertag)
		return CGI.escape(gamertag)
	end
end
