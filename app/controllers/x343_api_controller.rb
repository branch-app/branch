class X343ApiController < ApplicationController
	cattr_accessor :game_meta_data

	# Status Codes
	## 1001 - Server Down

	# requires
	require 'net/https'
	require 'open-uri'
	require 'uri'
	require 'json'
	require 'openssl'

	# human stupidity
	GAME = 'h4'
	LANGUAGE = 'en-us'
	SERVICES_LIST_URI = URI.parse('https://settings.svc.halowaypoint.com/RegisterClientService.svc/register/webapp/AE5D20DCFA0347B1BCE0A5253D116752')

	# private variables

	def self.UpdateServicesList
		response = unauthorized_request(SERVICES_LIST_URI, 'GET', nil)

		if response.code == '200'
			response = JSON.parse(response.body)

			services_list = response['ServiceList']
			settings = response['Settings']

			# save to database
			H4ServicesList.delete_all()

			services_list.each do |services_list_item|
				new_item = H4ServicesList.new
				new_item.name = services_list_item[0]
				new_item.url = services_list_item[1]
				new_item.list_type = 'service_list'
				new_item.save
			end

			settings.each do |services_list_item|
				new_item = H4ServicesList.new
				new_item.name = services_list_item[0]
				new_item.url = services_list_item[1]
				new_item.list_type = 'settings'
				new_item.save
			end
		end

	end

	def self.UpdateMetaData
		url = url_from_name('GetGameMetadata', 'service_list')
		url = full_url_with_defaults(url, nil)
		response = unauthorized_request(URI.parse(url), 'GET', nil)

		if response.code == '200'
			# save to database
			H4GameMetadata.delete_all()
			meta_data = H4GameMetadata.new
			meta_data.data = response.body
			meta_data.save

			$game_meta_data = JSON.parse(response.body)
		end
	end

	def self.UpdatePlaylists
		url = url_from_name('GetPlaylists', 'service_list')
		url = full_url_with_defaults(url, nil)
		response = authorized_request(URI.parse(url), 'GET', 'Spartan', nil)

		if response.code == '200'
			H4Playlists.delete_all
			playlist_model = H4Playlists.new
			playlist_model.data = response.body
			playlist_model.save
		end
	end

	def self.UpdateChallenges
		url = url_from_name('GetGlobalChallenges', 'service_list')
		url = full_url_with_defaults(url, nil)
		response = unauthorized_request(URI.parse(url), 'GET', nil)

		if response.code == '200'
			json = JSON.parse(response.body)

			if json['StatusCode'] != 1
				raise
			end

			H4GlobalChallenges.delete_all
			new_challenge = H4GlobalChallenges.new
			new_challenge.data = response.body
			new_challenge.save
		end
	end

	def self.GetMetaData
		# load from sql
		cached_meta = H4GameMetadata.first

		if cached_meta != nil && cached_meta.data != nil
			json = JSON.parse(cached_meta.data)
			return json
		else
			UpdateMetaData()

			cached_meta = H4GameMetadata.first

			if cached_meta != nil && cached_meta.data != nil
				json = JSON.parse(cached_meta.data)
				json
			end
		end
	end

	def self.GetServiceRecord(gamertag)
		gamertag_name = gamertag.to_s.downcase
		cached_sr = H4PlayerServicerecords.find_by_gamertag(gamertag_name)

		if cached_sr != nil && cached_sr.data != nil && cached_sr.updated_at + (60 * 8) > Time.now
			json = JSON.parse(cached_sr.data)
			return json
		else
			url = url_from_name('GetServiceRecord', 'service_list')
			url = full_url_with_defaults(url, { :gamertag => gamertag })
			response = authorized_request(URI.parse(url), 'GET', 'Spartan', nil)

			i = response.code

			if response.code == '200'
				data = JSON.parse(response.body)

				# check shit worked
				if data['StatusCode'] != 1
					return { :status_code => data['StatusCode'], :continue => 'no' }
				else
					old_cached = H4PlayerServicerecords.find_all_by_gamertag(gamertag_name)
					if old_cached != nil
						H4PlayerServicerecords.delete(old_cached)
					end

					cached_sr = H4PlayerServicerecords.new
					cached_sr.gamertag = gamertag_name
					cached_sr.data = response.body
					cached_sr.save

					data
				end
			else
				# try returning cache
				if cached_sr != nil
					return JSON.parse(cached_sr.data)
				else
					return { :status_code => 1001, :continue => 'no' }
				end
			end
		end
	end

	def self.GetPlayerCommendations(gamertag)
		gamertag_name = gamertag.to_s.downcase
		cached_com = H4PlayerCommendations.find_by_gamertag(gamertag_name)

		if cached_com != nil && cached_com.data != nil && cached_com.updated_at + (60 * 8) < Time.now
			json = JSON.parse(cached_com.data)
			return json
		else
			url = url_from_name('GetCommendations', 'service_list')
			url = full_url_with_defaults(url, { :gamertag => gamertag })
			response = authorized_request(URI.parse(url), 'GET', 'Spartan', nil)

			if response.code == '200'
				data = JSON.parse(response.body)

				# check shit worked
				if data['StatusCode'] != 1
					return { :status_code => data['StatusCode'], :continue => 'no' }
				else
					old_cached = H4PlayerCommendations.find_all_by_gamertag(gamertag_name)
					if old_cached != nil
						H4PlayerCommendations.delete(old_cached)
					end

					cached_com = H4PlayerCommendations.new
					cached_com.gamertag = gamertag_name
					cached_com.data = response.body
					cached_com.save

					data
				end
			else
				# try returning cache
				if cached_com != nil
					return JSON.parse(cached_com.data)
				else
					return { :status_code => 1001, :continue => 'no' }
				end
			end
		end
	end

	def self.GetPlayerMatches(gamertag, start_index, count, mode_id = 3, chapter_id = -1)
		gamertag_name = gamertag.to_s.downcase
		cached_match = H4PlayerRecentMatches.find_by_gamertag_and_start_index_and_count_and_mode_id_and_chapter_id(gamertag_name, start_index, count, mode_id, chapter_id)

		if cached_match != nil && cached_match.data != nil && cached_match.updated_at + (60 * 8) < Time.now
			json = JSON.parse(cached_match.data)
			return json
		else
			url = url_from_name('GetGameHistory', 'service_list')
			url += '?gamemodeid={gamemodeid}&count={count}&startat={startat}'
			if chapter_id != -1
				url += '&chapterid={chapterid}'
			end
			url = full_url_with_defaults(url, { :gamertag => gamertag, :count => count.to_s, :startat => start_index.to_s, :gamemodeid => mode_id.to_s, :chapterid => chapter_id.to_s })

			response = authorized_request(URI.parse(url), 'GET', 'Spartan', nil)

			if response.code == '200'
				data = JSON.parse(response.body)

				# check shit worked
				if data['StatusCode'] != 1
					return { :status_code => data['StatusCode'], :continue => 'no' }
				else
					old_cached = H4PlayerRecentMatches.find_by_gamertag_and_start_index_and_count(gamertag_name, start_index, count)
					if old_cached != nil
						H4PlayerRecentMatches.delete(old_cached)
					end

					cached_match = H4PlayerRecentMatches.new
					cached_match.gamertag = gamertag_name
					cached_match.data = response.body
					cached_match.start_index = start_index
					cached_match.count = count
					cached_match.mode_id = mode_id
					cached_match.chapter_id = chapter_id
					cached_match.save

					data
				end
			else
				# try returning cache
				if cached_match != nil
					return JSON.parse(cached_match.data)
				else
					return { :status_code => 1001, :continue => 'no' }
				end
			end
		end
	end

	def self.GetMatchDetails(gamertag, match_id)
		gamertag_name = gamertag.to_s.downcase
		cached_match = H4PlayerMatch.find_by_gamertag_and_game_id(gamertag_name, match_id)

		if cached_match != nil && cached_match.data != nil
			json = JSON.parse(cached_match.data)
			return json
		else
			url = url_from_name('GetGameDetails', 'service_list')
			url = full_url_with_defaults(url, { :gamertag => gamertag, :gameid => match_id })
			response = authorized_request(URI.parse(url), 'GET', 'Spartan', nil)

			if response.code == '200'
				data = JSON.parse(response.body)

				# check shit worked
				if data['StatusCode'] != 1
					return { :status_code => data['StatusCode'], :continue => 'no' }
				else
					old_cached = H4PlayerMatch.find_by_gamertag_and_game_id(gamertag_name, match_id)
					if old_cached != nil
						H4PlayerMatch.delete(old_cached)
					end

					cached_match = H4PlayerMatch.new
					cached_match.gamertag = gamertag_name
					cached_match.data = response.body
					cached_match.game_id = match_id
					cached_match.save

					data
				end
			else
				# try returning cache
				if cached_match != nil
					return JSON.parse(cached_match.data)
				else
					return { :status_code => 1001, :continue => 'no' }
				end
			end
		end
	end

	def self.GetPlayerModel(gamertag, size)
		url = url_from_name('GetSpartanImage', 'service_list') # https://spartans.svc.halowaypoint.com/players/{gamertag}/{game}/spartans/{pose}?target={size}
		url = full_url_with_defaults(url, { :gamertag => gamertag, :pose => 'fullbody', :size => size })

		url
	end

	def self.GetPlayerChallenges(gamertag)

		cached_result = H4PlayerChallenges.find_by_gamertag(gamertag)

		if cached_result != nil && cached_result.data != nil && cached_result.updated_at + 8*60 > Time.now
			# return cached data
			json = JSON.parse(cached_result.data)

			json
		else
			url = url_from_name('GetPlayerChallenges', 'service_list')
			url = full_url_with_defaults(url, { :gamertag => gamertag })
			response = authorized_request(URI.parse(url), 'GET', 'Spartan', nil)

			if response.code == '200'
				data = JSON.parse(response.body)

				# check gamertag exists
				if data['StatusCode'] != 1
					'player_not_found'
				elsif data['StatusCode'] != 0
			    raise
				end

				# cache
				if cached_result != nil
					cached_result.data = response.body
				else
					cached_result = H4PlayerChallenges.next
					cached_result.data = response.body
				end

				data
			else
				# try returning cache
				if cached_result != nil
					return JSON.parse(cached_result.data)
				else
					return { :status_code => 1001, :continue => 'no' }
				end
			end
		end
	end


	### <summary>
	### helpers
	### </summary>
	def self.mapmeta_from_id(id, meta)
		if meta == nil
			meta = GetMetaData()['MapsMetadata']['Maps']
		end

		meta.each do |map|
			if map['Id'] == id
				return map
			end
		end
	end

	def self.get_metadata
		JSON.parse(H4GameMetadata.first.data)
	end

	def self.asset_url_generator_basic(base_url, asset_url, size)
		true_base = H4ServicesList.find_by_name_and_list_type(base_url, 'settings')

		real_url = "#{true_base.url}#{asset_url.gsub('{size}', size)}"

		real_url
	end


	### <summary>
	### callz
	### </summary>
	def self.unauthorized_request(uri, request_type, optional_headers)
		request = nil
		if request_type == 'GET'
			request = Net::HTTP::Get.new(uri.path)
		elsif request_type == 'POST'
			request = Net::HTTP::Post.new(uri.path)
		else
			raise
		end

		request['Accept'] = 'application/json'
		if optional_headers != nil
			# set dem headerz
			optional_headers.each do |header|
				request[header[0]] = header[1]
			end
		end

		response = Net::HTTP.start(uri.hostname, uri.port, :use_ssl => uri.scheme == 'https', :verify_mode => OpenSSL::SSL::VERIFY_NONE) {|http|
			http.request(request)
		}

		response
	end

	def self.authorized_request(uri, request_type, auth_type, optional_headers)
		auth = H4ApiAuthenticationVault.first

		request = nil
		if request_type == 'GET'
			request = Net::HTTP::Get.new(uri.path)
		elsif request_type == 'POST'
			request = Net::HTTP::Post.new(uri.path)
		else
			raise
		end

		request['Accept'] = 'application/json'
		if optional_headers != nil
			# set dem headerz
			optional_headers.each do |header|
				request[header[0]] = header[1]
			end
		end
		if auth_type == 'Spartan'
			request['X-343-Authorization-Spartan'] = auth.spartan_token
		elsif auth_type == 'WLID'
			request['X-343-Authorization-WLID'] = 'v1=' + auth.wlid_access_token
		end

		response = Net::HTTP.start(uri.hostname, uri.port, :use_ssl => uri.scheme == 'https', :verify_mode => OpenSSL::SSL::VERIFY_NONE) {|http|
			http.request(request)
		}

		response
	end

	def self.full_url_with_defaults(url, custom_defaults)
		url = url.gsub('{language}', LANGUAGE)
		url = url.gsub('{game}', GAME)

		if custom_defaults != nil
			custom_defaults.each do |default|
				url = url.gsub("{#{default[0]}}", default[1])
			end
		end

		url = url.gsub(' ', '%20')

		url
	end

	def self.url_from_name(name, type)
		entry = H4ServicesList.find_by_name_and_list_type(name, type)

		if entry == nil
			raise
		end

		entry.url
	end

	def self.error_message_from_status_code(status_code, param1 = '')
		case status_code
			when 1001
				return 'The Halo Waypoint API is down, we can only load cached data. Sorry about that.'
			when 4
				return "The specified gamertag `#{param1}` has no Halo 4 game history."
			else
				return 'Unknown Status Code'
		end
	end
end

