class Halo4::HomeController < ApplicationController
	include Halo4::HomeHelper
	before_filter :get_gamertag

	@gamertag = nil
	def get_gamertag
		@metadata = H4Api.get_meta_data()
		
		# hacky fix to get controller name/action, huehuehue
		if (!(controller_name() == 'home' && (action_name() == 'index' || action_name() == 'favourite')))
			@gamertag = params[:gamertag]

			new_gamertag = GamertagReplacement.find_by_replacement(@gamertag)
			if (new_gamertag != nil)
				@gamertag = new_gamertag.target
			end

			# pull stats
			@service_record = H4Api.get_player_service_record(@gamertag)

			if (@service_record == nil || @service_record[:continue] == false || @service_record["StatusCode"] != 1)
				set_flash_message('failure', 'Oops...', "We couldn't load stats for the gamertag #{@gamertag}, sorry :(")
				redirect_to(root_url())
				return
			end
		end
	end

	def index
		
	end

	def favourite
		game = H4Game.find_by_game_id(params[:favourite][:game_id])

		if (game == nil)
			render json: { state: nil, success: false, error: { name: 'invalid_game_shit', desc: "The Game ID seems to of broken.. fuck." } }
			return
		end

		owner = game.h4_service_record.gamertag.gamertag

		if (current_user() == nil)
			render json: { state: nil, success: false, error: { name: 'invalid_user_shit', desc: "You're not signed in or you're banned. sign in m8." } }
			return
		end

		# check shit
		current_favourite = H4Favourite.find_by_game_id(params[:favourite][:game_id])
		if (current_favourite != nil)
			favourite_parent = current_favourite.favourite

			favourite_parent.destroy()
			current_favourite.destroy()

			render json: { state: 'favourite', success: true, response: { happy_message: "You managed to un-favourite the game, well done." } }
			return
		else
			#begin
				game_data = H4Api.get_player_game(owner, game.game_id)['Game']	
				top_player = game_data['Players'].sort_by { |p| p['Standing'] }[0]

				game_variant_id = 0
				game_variant_name = ""
				map_id = game_data['MapId']
				map_variant_name = ""
				mvp_gamertag = top_player['Gamertag']
				mvp_kd = calculate_kd(top_player['Kills'], top_player['Deaths'])
				mvp_kills = top_player['Kills']

				# Customs
				# Matchmaking
				if (game_data['ModeId'] == 3 || game_data['ModeId'] == 6)
					game_variant_id = game_data['GameBaseVariantId']
					game_variant_name = game_data['GameVariantName']
					map_variant_name = game_data['MapVariantName'] || game_data['MapName']
				end

				# Spartan Ops
				# Campaign
				if (game_data['ModeId'] == 4 || game_data['ModeId'] == 5)
					game_variant_id = game_data['Difficulty']
					game_variant_name = get_difficulty_from_base_id(game_data['Difficulty'])['Name']
					map_variant_name = game_data['MapName']
				end

				favourite_parent = Favourite.new(user_id: current_user().id)
				if (!favourite_parent.save())
					render json: { state: 'favourite', success: false, error: { name: 'error_saving_model', desc: "Parent model didn't save, fuck." } }
					return
				end

				favourite = H4Favourite.new(mode_id: game_data['ModeId'].to_i, game_variant_name: game_variant_name, game_id: game.game_id, favourite_id: favourite_parent.id, game_variant_id: game_variant_id, game_variant_name: game_variant_name, map_id: map_id, map_variant_name: map_variant_name, mvp_gamertag: mvp_gamertag, mvp_kd: mvp_kd, mvp_kills: mvp_kills)
				if (!favourite.save())
					render json: { state: 'favourite', success: false, error: { name: 'error_saving_model', desc: "Parent model didn't save, fuck." } }
					return
				end

				render json: { state: 'unfavourite', success: true, response: { happy_message: "You managed to favourite the game, well done." } }
				return
			#rescue
				render json: { state: 'favourite', success: false, error: { name: 'error_loading_game', desc: "Couldn't load game JSON." } }
				return
			#end
		end


		# render json: { state: 'following', success: true, response: { happy_message: "You're now following #{user_a.username}. Good on you man :)" } }
		# return

	end
end
