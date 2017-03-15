require('addressable/uri')

class Halo4::HomeController < ApplicationController
	before_action :require_gamertag, except: :index
	before_action :setup_area

	def index
		
	end

	private

	def setup_area
		@area = {
			name: 'Halo 4',
			name_slug: 'halo-4',
			exp: 'Halo',
			exp_slug: 'halo',
		}
	end

	def require_gamertag
		@gamertag = Addressable::URI.escape(params[:gamertag])

		begin
			@identity = ServiceClient.instance.get('service-xboxlive', "/identity/gamertag(#{@gamertag})/")

			# TODO: Run these concurrently
			@service_record = ServiceClient.instance.get('service-halo4', "/identity/xuid(#{@identity['xuid']})/service-record")
			@recent_matches = ServiceClient.instance.get('service-halo4', "/identity/xuid(#{@identity['xuid']})/recent-matches?modeId=3&count=5")

		rescue BranchError => e
			case e.code
			when 'invalid_identity'
			when 'identity_doesnt_exist'
				flash[:warning] = 'The provided Gamertag doesn\'t exist.'
				redirect_to(controller: 'home', action: 'index')
				return

			when 'waypoint_content_not_found'
				# TODO
				flash[:warning] = 'REPLACE_ME'
				return

			when 'waypoint_no_data'
				flash[:warning] = "Sorry, #{@identity['gamertag']} has never played Halo 4."
				redirect_to(controller: 'xbox-live/profile', action: 'profile', gamertag: @identity['gamertag'])
				return

			else
				flash[:danger] = "An unknown error occured."
			end

			redirect_to(controller: 'home', action: 'index')
		end
	end
end
