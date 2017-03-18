class Halo4::MatchesController < Halo4::HomeController
	@@game_modes = {
		'war-games' => 3,
		'campaign' => 4,
		'spartan-ops' => 5,
		'custom-games' => 6,
	}

	def recent_matches
		@title = "Branch - #{@identity['gamertag']}'s ** Match History"

		count = 25
		@slug = params[:slug]
		@mode = @@game_modes[@slug]
		if @mode == nil
			flash[:warning] = "That game mode doesn't exist.'"
			return redirect_to(action: 'recent_mathes', controller: 'halo4#matches', gamertag: @identity['gamertag'], slug: 'war-games')
		end

		@page = (params[:page] || '1').to_i
		@page = 1 if @page < 1
		@sidebar = { recent_matches: 'active' }
		case @mode
			when 3
				@sidebar[:war_games_mode] = 'active'
			when 4
				@sidebar[:campaign_mode] = 'active'
			when 5
				@sidebar[:spartan_ops_mode] = 'active'
			when 6
				@sidebar[:custom_games_mode] = 'active'
		end

		begin
			@recent_matches = ServiceClient.instance.get('service-halo4', "/identity/xuid(#{@identity['xuid']})/recent-matches?modeId=#{@mode}&count=#{count}&startAt=#{(@page - 1) * count}")
		rescue BranchError => e
			case e.code
				when 'waypoint_no_data'
					raise

				else
					raise
			end

			return redirect_to(controller: 'home', action: 'index')
		end
	end

	def match
	end
end
