class Halo4::HomeController < ApplicationController
	include Halo4::HomeHelper
	before_filter :get_gamertag

	@gamertag = nil
	def get_gamertag
		# hacky fix to get controller name/action, huehuehue
		if (!(controller_name() == 'home' && action_name() == 'index'))
			@gamertag = params[:gamertag]

			new_gamertag = GamertagReplacement.find_by_replacement(@gamertag)
			if (new_gamertag != nil)
				@gamertag = new_gamertag.target
			end

			# pull stats
			@service_record = H4Api.get_player_service_record(@gamertag)
			@metadata = H4Api.get_meta_data()

			if (@service_record == nil || @service_record[:continue] == false || @service_record["StatusCode"] != 1)
				set_flash_message('failure', 'Oops...', "We couldn't load stats for the gamertag #{@gamertag}, sorry :(")
				redirect_to(root_url())
				return
			end
		end
	end

	def index
		
	end

end
