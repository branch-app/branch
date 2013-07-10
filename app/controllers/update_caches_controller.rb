class UpdateCachesController < ApplicationController
	include I343Auth

	def devcacheupdate
		# authorize
		I343Auth.update_authentication

		# call
		I343ApiH4.update_services_list
		I343ApiH4.get_meta_data
		I343ApiH4.get_playlist_data
		I343ApiH4.get_challenge_data

		flash[:notice] = 'Halo 4 Authentication, Halo 4 Services List, Halo 4 Meta Data, Halo 4 Challenges, Halo 4 Playlists have all been updated. Well fucking done.'
		flash[:type] = 'success'
		redirect_to root_url
	end
end