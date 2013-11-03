class Halo4::PlaylistsController < Halo4::HomeController
	def index
		@playlist_data = H4Api.get_playlist_data()['Playlists']

		@modified_playlist_data = [ ]

		@playlist_data.each do |playlist|
			insert_data = (@modified_playlist_data[playlist['ModeId']] == nil)
			if (insert_data)
				@modified_playlist_data[playlist['ModeId']] = {
					total_population: 0,
					mode_id: playlist['ModeId'],
					mode_name: playlist['ModeName'],
					playlists: [ ]
				}
			end

			@modified_playlist_data[playlist['ModeId']][:total_population] += playlist['PopulationCount']

			playlist_orientation = H4PlaylistOrientation.find_by_playlist_id(playlist['Id'])
			if (playlist_orientation == nil)
				playlist['Orientation'] = 'Unknown'
			elsif (playlist_orientation.is_team)
				playlist['Orientation'] = 'Team'
			elsif (playlist_orientation.is_individual)
				playlist['Orientation'] = 'Individual'
			end

			@modified_playlist_data[playlist['ModeId']][:playlists] << playlist
		end

		@modified_playlist_data.delete_if { |p| (p == nil) }
		@modified_playlist_data.map{ |p| p[:playlists].sort_by{ |pl| pl['Id'] } }
		@modified_playlist_data.sort_by{ |p| p[:mode_id] }
	end
end