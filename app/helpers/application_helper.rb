module ApplicationHelper
	#-- Html Helpers --#
	def gamertag_to_html(gamertag)
		return CGI.escape(gamertag)
	end

	def gamertag_to_leaf_html(gamertag)
		return gamertag.gsub(' ', '_')
	end

	#-- Social Shit --#
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

	#-- Branch Helpers --#
	def gamertag_validation(gamertag)
		replacement = GamertagReplacement.find_by_target(gamertag)
		if replacement == nil
			return gamertag
		else
			return replacement.replacement
		end
	end

	def create_return_url()
		return "?return_url=#{CGI.escape(request.url)}"
	end

	def flash_class(level)
		case level
			when :success then "success"
			when :failure then "danger"
			when :warning then "warning"
			when :info then "info"
		end
	end


	#-- Date Helpers --#
	def parse_datetime(date_string, in_format = '%Y-%m-%dT%H:%M:%SZ', out_format = '%d/%m/%Y')
		return DateTime.strptime(date_string, in_format).strftime(out_format)
	end

	def duration_to_seconds(duration_str)
		duration = Time.strptime(duration_str, "%H:%M:%S")

		return duration.seconds_since_midnight.to_i
	end

	#-- Graph Helpers --#
	def generate_graph_from_single(dataset, duration)
		output = ""
		running_total = 0
		duration_sec = duration_to_seconds(duration)
		duration_sec.times do |i|
			if (i == 0 || (i + 1) == duration_sec)
				output += "{ x: '#{i}', a: '#{ running_total }' },"
			end

			dataset.each do |medal_entry|
				if (medal_entry['Time'] == i)
					running_total += medal_entry['Ticks']
					output += "{ x: '#{i}', y: '#{ running_total }' },"
				end
			end
		end

		output = output[0..-1]
	end

	#-- Maths Helpers --#
	def calculate_kd(kills, deaths, round_to = 2)
		if (kills <= 0 || deaths <= 0)
            return 0.0
        else
            return (kills.to_f() / deaths.to_f()).to_f().round(round_to)
        end
	end

	def calculate_spread(kills, deaths)
		return (kills - deaths)
	end
end
