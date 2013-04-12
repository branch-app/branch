class X343ApiController < ApplicationController
	cattr_accessor :game_meta_data

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
		response = JSON.parse(response.body)

		services_list = response['ServiceList']
		settings = response['Settings']

		# save to database
		ServicesList.delete_all()

		services_list.each do |services_list_item|
			new_item = ServicesList.new
			new_item.name = services_list_item[0]
			new_item.url = services_list_item[1]
			new_item.list_type = 'service_list'
			new_item.save
		end

		settings.each do |services_list_item|
			new_item = ServicesList.new
			new_item.name = services_list_item[0]
			new_item.url = services_list_item[1]
			new_item.list_type = 'settings'
			new_item.save
		end

	end

	def self.UpdateMetaData
		url = url_from_name('GetGameMetadata', 'service_list')
		url = full_url_with_defaults(url, nil)
		response = unauthorized_request(URI.parse(url), 'GET', nil)

		# save to database
		GameMetaData.delete_all()
		meta_data = GameMetaData.new
		meta_data.data = response.body
		meta_data.save

		$game_meta_data = JSON.parse(response.body)
	end

	def self.UpdatePlaylists
		url = url_from_name('GetPlaylists', 'service_list')
		url = full_url_with_defaults(url, nil)
		response = authorized_request(URI.parse(url), 'GET', 'Spartan', nil)

		Playlists.delete_all
		playlist_model = Playlists.new
		playlist_model.data = response.body
		playlist_model.save
	end

	def self.UpdateChallenges
		url = url_from_name('GetGlobalChallenges', 'service_list')
		url = full_url_with_defaults(url, nil)
		response = unauthorized_request(URI.parse(url), 'GET', nil)

		json = JSON.parse(response.body)

		if json['StatusCode'] != 1
			raise
		end

		GlobalChallenges.delete_all
		new_challenge = GlobalChallenges.new
		new_challenge.data = response.body
		new_challenge.save
	end

	def self.GetMetaData
		# load from sql
		cached_meta = GameMetaData.first

		if cached_meta != nil && cached_meta.data != nil
			json = JSON.parse(cached_meta.data)
			return json
		else
			UpdateMetaData()

			cached_meta = GameMetaData.first

			if cached_meta != nil && cached_meta.data != nil
				json = JSON.parse(cached_meta.data)
				json
			end
		end
	end

	def self.GetServiceRecord(gamertag)
		url = url_from_name('GetServiceRecord', 'service_list')
		url = full_url_with_defaults(url, { :gamertag => gamertag })
		response = authorized_request(URI.parse(url), 'GET', 'Spartan', nil)

		data = JSON.parse(response.body)

		data
	end

	def self.GetPlayerModel(gamertag, size)
		url = url_from_name('GetSpartanImage', 'service_list') # https://spartans.svc.halowaypoint.com/players/{gamertag}/{game}/spartans/{pose}?target={size}
		url = full_url_with_defaults(url, { :gamertag => gamertag, :pose => 'fullbody', :size => size })

		url
	end

	def self.GetPlayerChallenges(gamertag)

		cached_result = PlayerChallenges.find_by_gamertag(gamertag)

		if cached_result != nil && cached_result.data != nil && cached_result.updated_at + 8*60 > Time.now
			# return cached data
			json = JSON.parse(cached_result.data)

			json
		else
			url = url_from_name('GetPlayerChallenges', 'service_list')
			url = full_url_with_defaults(url, { :gamertag => gamertag })
			response = authorized_request(URI.parse(url), 'GET', 'Spartan', nil)

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
				cached_result = PlayerChallenges.next
				cached_result.data = response.body
			end

			data
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
		JSON.parse(GameMetaData.first.data)
	end

	def self.asset_url_generator_basic(base_url, asset_url, size)
		true_base = ServicesList.find_by_name_and_list_type(base_url, 'settings')

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
		auth = AuthenticationVault.first

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

		url
	end

	def self.url_from_name(name, type)
		entry = ServicesList.find_by_name_and_list_type(name, type)

		if entry == nil
			raise
		end

		entry.url
	end
end

