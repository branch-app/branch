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
			puts public_path
			puts asset
			return image_url(public_path)
		end

		url = Halo4Client.instance.metadata_options['settings'][base_url]
		return url + asset_url
	end

	def resolve_spartan_image(gamertag, pose = 'fullbody', size = 'large')
		return "https://spartans.svc.halowaypoint.com/players/#{gamertag}/h4/spartans/#{pose}?target=#{size}"
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
		@recent_matches['games'].to_json
	end

	def most_recent_match_image
		resolve_asset_url(@recent_matches['games'][0]['mapImageUrl']) if !@recent_matches['games'][0].nil?
	end

	def has_recent_matches
		!@recent_matches['games'][0].nil?
	end
end
