module Halo4::HomeHelper
	def resolve_asset_url(asset, size = 'large')
		asset_url = asset['assetUrl'].gsub('{size}', size.to_s)
		base_url = asset['baseUrl']

		public_path = "games/halo4/#{base_url}/#{asset_url}"

		case base_url
		when 'H4MapAssets'
			public_path = public_path.gsub('.png', '.jpg')
		end

		if public_asset_exists?(public_path)
			return image_url(public_path)
		end

		url = Halo4Client.instance.metadata_options['settings'][base_url]
		return '' if url == nil
		return url + asset_url
	end

	def resolve_spartan_image(gamertag, pose = 'fullbody', size = 'large')
		return "https://spartans.svc.halowaypoint.com/players/#{gamertag}/h4/spartans/#{pose}?target=#{size}"
	end

	def parse_duration(str)
		regex_str = /(?:(?<days>\d{1,3}?)\.)?(?<hours>\d{1,2}):(?<minutes>\d{1,2}):(?<seconds>\d{1,2})/i

		days, hours, minutes, seconds = str.match(regex_str).captures
		days = days.to_i * 1.day.to_i
		hours = hours.to_i * 1.hour.to_i
		minutes = minutes.to_i * 1.minute.to_i
		seconds = seconds.to_i

		duration = Duration.new(days + hours + minutes + seconds)
		output = ''
		output += "#{pluralize(duration.weeks, 'week')}, " if duration.weeks > 0
		output += "#{pluralize(duration.days, 'day')}, " if duration.days > 0
		output += "#{pluralize(duration.hours, 'hour')}, " if duration.hours > 0
		output += "#{output.length > 0 ? ' and' : ''} #{pluralize(duration.minutes, 'minute')}, " if duration.minutes > 0
		# output += "#{pluralize(duration.minutes, 'second')}" if duration.seconds > 0

		return output.chomp(', ')
	end

	def friendly_result(data)
		return 'Did Not Finish', 'dnf' if !data['completed']

		case data['result']
			when 0
				return 'Lost', 'lost'
			when 1
				return 'Draw', 'draw'
			when 2
				return 'Won', 'won'
		end

		return 'Unknown', 'unk'
	end

	def humanize_difficulty(difficulty)
		case difficulty
			when 0
				return 'Easy'
			when 1
				return 'Normal'
			when 2
				return 'Heroic'
			when 3
				return 'Legendary'
		end
	end

	# These are used on the service-record header on all pages
	def total_kills
		kills = 0
		return kills if @service_record['gameModes'].empty?
		@service_record['gameModes'].map do |mode|
			kills += mode['totalKills'].to_i
		end

		return kills
	end

	def total_deaths
		deaths = 0
		return deaths if @service_record['gameModes'].empty?
		@service_record['gameModes'].map do |mode|
			deaths += mode['totalDeaths'].to_i
		end

		return deaths
	end

	def total_games_started
		games_started = 0
		return games_started if @service_record['gameModes'].empty?
		@service_record['gameModes'].map do |mode|
			games_started += mode['totalGamesStarted'].to_i
		end

		return games_started
	end

	def generate_recent_match_json
		@header_matches['games'].to_json
	end

	def most_recent_match_image
		resolve_asset_url(@header_matches['games'][0]['mapImageUrl']) if !@header_matches['games'][0].nil?
	end

	def has_recent_matches
		!@header_matches['games'][0].nil?
	end
end
