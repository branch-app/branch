require 'json'
require 'openssl'
require 'httparty'
require 'cgi'

module I343Auth

	def self.update_authentication
		init

		unless Rails.env.local_stage?
			url = "http://authentication.xeraxic.com/api/halo4/?email=#{CGI.escape(ENV['SEC_E'].to_s)}&password=#{CGI.escape(ENV['SEC_P'].to_s)}"
			response = HTTParty.get(url)

			if (response != nil || response.code == 200)
				tokens = JSON.parse(response.body)

				# get dem codes to tha db
				H4ApiAuthenticationVault.delete_all
				auth_vault = H4ApiAuthenticationVault.new
				auth_vault.spartan_token = tokens['SpartanToken']
				auth_vault.save
			end
		end
	end

	@is_initalized = false
	def self.init
		return if @is_initalized

		@is_initalized = true
	end
end