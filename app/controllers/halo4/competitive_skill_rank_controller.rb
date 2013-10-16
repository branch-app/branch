class Halo4::CompetitiveSkillRankController < Halo4::HomeController
	def index
		@team_playlists = [ ]
		@individual_playlists = [ ]

		playlist_orientations = H4PlaylistOrientation.all
		@service_record['SkillRanks'].each do |skill_rank|
			playlist_is_team = check_playlist_orientation_is_team(skill_rank, playlist_orientations)
			skill_rank['CurrentSkillRank'] = 0 if skill_rank['CurrentSkillRank'] == nil

			if (playlist_is_team)
				@team_playlists << skill_rank
			elsif (!playlist_is_team)
				@individual_playlists << skill_rank
			end
		end

		@team_playlists.sort_by!{ |p| -p['CurrentSkillRank'] }
		@individual_playlists.sort_by!{ |p| -p['CurrentSkillRank'] }
	end

	def details
		playlist_id = params[:id].to_i

		@current_skill_rank = nil
		@service_record['SkillRanks'].each do |skill_rank|
			if (skill_rank['PlaylistId'] == playlist_id)
				@current_skill_rank = skill_rank
			end
		end

		@playlist = nil
		@metadata['PlaylistsMetadata']['Playlists'].each do |playlist|
			if (playlist['Id'] == playlist_id)
				@playlist = playlist
			end
		end

		if (@current_skill_rank == nil)
			flash[:failure] = 'Unable to find this playlist, it could of been deleted by 343, or the URL has become corrupt.'
			redirect_to(halo4_csr_path(gamertag: @service_record['Gamertag']))
			return
		end
	end

	private
		def check_playlist_orientation_is_team(playlist, orientations)
			orientations.each do |orientation|
				if (orientation.playlist_id == playlist['PlaylistId'])
					return orientation.is_team
				end
			end

			return nil
		end
end
