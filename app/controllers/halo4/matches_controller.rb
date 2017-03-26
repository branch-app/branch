class Halo4::MatchesController < Halo4::HomeController
	include Halo4::HomeHelper
	@@game_modes = {
		'war-games' => {id: 3, name: 'War Games'},
		'campaign' => {id: 4, name: 'Campaign'},
		'spartan-ops' => {id: 5, name: 'Spartan Ops'},
		'custom-games' => {id: 6, name: 'Custom Games'},
	}

	def recent_matches
		count = 25
		@slug = params[:slug]
		@mode_info = @@game_modes[@slug]
		@mode = @mode_info[:id]
		@mode_name = @mode_info[:name]
		if @mode == nil
			flash[:warning] = "That game mode doesn't exist.'"
			return redirect_to(action: 'recent_mathes', controller: 'halo4#matches', gamertag: @identity['gamertag'], slug: 'war-games')
		end

		@title = "Branch - #{@identity['gamertag']}'s #{@mode_name} Match History"
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
			@recent_matches = ServiceClient.instance.get('service-halo4', "/identity/xuid(#{@identity['xuid']})/matches/#{}?modeId=#{@mode}&count=#{count}&startAt=#{(@page - 1) * count}")
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
		@match_id = params[:id]
		@sidebar = { recent_matches: 'active' }

		begin
			@match = ServiceClient.instance.get('service-halo4', "/matches/#{@match_id}")
			@game = @match['game']
			case @game['modeId']
				when 3
					@sidebar[:war_games_mode] = 'active'
					@title = "Branch - #{@identity['gamertag']}'s Match - #{@game['gameVariantName']} on #{@game['mapVariantName']}"
				when 4
					@sidebar[:campaign_mode] = 'active'
					@title = "Branch - #{@identity['gamertag']}'s Match - #{@game['mapName']} on #{humanize_difficulty(@game['difficulty'])}"
				when 5
					@sidebar[:spartan_ops_mode] = 'active'
					@title = "Branch - #{@identity['gamertag']}'s Match - #{@game['mapName']} on #{humanize_difficulty(@game['difficulty'])}"
				when 6
					@sidebar[:custom_games_mode] = 'active'
					@title = "Branch - #{@identity['gamertag']}'s Match - #{@game['gameVariantName']} on #{@game['mapVariantName']}"
			end
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
end
