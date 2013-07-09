require 'uri'
require 'json'
require 'openssl'
require 'httparty'
require 'aws-sdk'

module I343ApiH4
	include S3Storage

	# woah, consts
	GAME = 'h4'
	GAME_LONG = 'halo4'
	LANGUAGE = 'en-us'
	SERVICES_LIST_URL = 'https://settings.svc.halowaypoint.com/RegisterClientService.svc/register/webapp/AE5D20DCFA0347B1BCE0A5253D116752'


	# Module Api Update Calls
	def self.update_services_list
		init
		
		response = unauthorized_request(SERVICES_LIST_URL, 'GET', nil)
		return unless validate_response(response)

		response = JSON.parse(response.body)

		services_list = response['ServiceList']
		settings = response['Settings']

		# save to database
		H4ServicesList.delete_all()

		$services_list = { service_list: { }, settings: { } }

		services_list.each do |services_list_item|
			new_item = H4ServicesList.new
			new_item.name = services_list_item[0]
			new_item.url = services_list_item[1]
			new_item.list_type = 'service_list'
			new_item.save

			$services_list[:service_list][services_list_item[0]] = services_list_item[1] 
		end

		settings.each do |services_list_item|
			new_item = H4ServicesList.new
			new_item.name = services_list_item[0]
			new_item.url = services_list_item[1]
			new_item.list_type = 'settings'
			new_item.save

			$services_list[:settings][services_list_item[0]] = services_list_item[1] 
		end
	end

	def self.update_meta_data
		init
		
		url = url_from_name('GetGameMetadata', 'service_list')
		url = full_url_with_defaults(url, nil)

		response = unauthorized_request(url, 'GET', nil)
		return unless validate_response(response)

		# save to s3
		S3Storage.push(GAME_LONG, 'other', 'metadata', response.body)

		$game_meta_data = JSON.parse response.body
		$game_meta_data
	end

	def self.update_playlist_data
		init

		url = url_from_name('GetPlaylists', 'service_list')
		url = full_url_with_defaults(url, nil)

		response = authorized_request(url, 'GET', 'Spartan', nil)
		return unless validate_response(response)

		# save to s3
		S3Storage.push(GAME_LONG, 'other', 'playlist_data', response.body)

		JSON.parse response.body
	end

	def self.update_challenge_data
		init

		url = url_from_name('GetGlobalChallenges', 'service_list')
		url = full_url_with_defaults(url, nil)

		response = unauthorized_request(url, 'GET', nil)
		return unless validate_response(response)

		# save to s3
		S3Storage.push(GAME_LONG, 'other', 'challenge_data', response.body)

		JSON.parse response.body
	end


	# Module Api Get Calls
	def self.get_meta_data
		cached_data = S3Storage.pull(GAME_LONG, 'other', 'metadata')

		unless cached_data == nil
			JSON.parse cached_data
		else
			update_metadata
		end
	end


	# Module Api Helpers
	def self.mapmeta_from_id(id, meta)
		init
		
		meta = GetMetaData()['MapsMetadata']['Maps'] if meta == nil
		meta.each do |map|
			return map if map['Id'] == id
		end
	end

	def self.get_metadata
		init
		
		JSON.parse(H4GameMetadata.first.data)
	end

	def self.asset_url_generator_basic(base_url, asset_url, size)
		init
		
		"#{$services_list[:settings][base_url]}#{asset_url.gsub('{size}', size)}"
	end


	# Module Helpers
	def self.validate_response(response)
		init
		
		false if response == nil || response.code != 200
		true
	end


	# Used for URL Modification and Server Calls
	def self.unauthorized_request(url, request_type, headers)
		init
		nil if Rails.env.local_stage?

		if headers == nil
			headers = { }
		end
		headers['Accept'] = 'application/json'

		response = nil
		if request_type == 'GET'
			response = HTTParty.get(url, :headers => headers)
		elsif request_type == 'POST'
			response = HTTParty.post(url, :headers => headers)
		else
			raise
		end

		response
	end

	def self.authorized_request(url, request_type, auth_type, headers)
		init
		nil if Rails.env.local_stage?

		auth = H4ApiAuthenticationVault.first

		if headers == nil
			headers = { }
		end
		if auth_type == 'Spartan'
			headers['X-343-Authorization-Spartan'] = auth.spartan_token
		elsif auth_type == 'WLID'
			headers['X-343-Authorization-WLID'] = 'v1=' + auth.wlid_access_token
		end
		headers['Accept'] = 'application/json'

		response = nil
		if request_type == 'GET'
			response = HTTParty.get(url, :headers => headers)
		elsif request_type == 'POST'
			response = HTTParty.post(url, :headers => headers)
		else
			raise
		end

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

		url.gsub(' ', '%20')
	end

	def self.url_from_name(name, type)
		entry = $services_list[type.to_sym][name]
		raise if entry == nil
		entry
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


	# Module Initalization
	@is_initalized = false
	def self.init
		return if @is_initalized

		@is_initalized = true
	end
end