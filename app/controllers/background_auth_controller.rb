class BackgroundAuthController < ApplicationController
	require 'net/https'
	require 'uri'
	require 'json'

	def self.UpdateAuthentication
		unless Rails.env.local_stage?
			loop = true

			while loop
				url = URI.parse('http://h4tokengen.xeraxic.com/api/authentication/' + ENV['PARAM1'])
				req = Net::HTTP::Get.new(url.path)
				res = Net::HTTP.start(url.host, url.port) {|http|
					http.request(req)
				}

				if res.code == '200'
					loop = false

					json = res.body.gsub('\\"', '"')
					json = json.gsub /^"|"$/, ''
					tokens = JSON.parse(json)

					H4ApiAuthenticationVault.delete_all
					# we have no existing auth entries, create a new one
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
end