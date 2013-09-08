require 'json'
require 'openssl'
require 'httparty'

module I343Auth

	def self.update_authentication
		init

		loop = true
		while loop
			unless Rails.env.local_stage?
				url = 'http://h4tokengen.xeraxic.com/api/authentication/' + ENV['SEC_TOKEN'].to_s
				response = HTTParty.get(url)

				if response != nil || response.code == 200
					loop = false

					json = response.body.gsub('\\"', '"')
					json = json.gsub /^"|"$/, ''
					tokens = JSON.parse(json)

					# get dem codes to tha db
					H4ApiAuthenticationVault.delete_all
					auth_vault = H4ApiAuthenticationVault.new
					auth_vault.spartan_token = tokens['X343Tokens']['SpartanToken']
					auth_vault.wlid_access_token = tokens['WLIDTokens']['AccessToken']
					auth_vault.wlid_authentication_token = tokens['WLIDTokens']['AuthenticationToken']
					auth_vault.wlid_expire = tokens['WLIDTokens']['Expires']
					auth_vault.save
				end
			end
		end
	end

	@is_initalized = false
	def self.init
		return if @is_initalized

		@is_initalized = true
	end
end