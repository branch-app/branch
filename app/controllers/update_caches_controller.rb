class UpdateCachesController < ApplicationController
	def devcacheupdate
		# authorize
		BackgroundAuthController.UpdateAuthentication

		# call
		X343ApiController.UpdateServicesList
		X343ApiController.UpdateMetaData
		X343ApiController.UpdateChallenges
		X343ApiController.UpdatePlaylists

		flash[:notice] = 'Halo 4 Authentication, Halo 4 Services List, Halo 4 Meta Data, Halo 4 Challenges, Halo 4 Playlists have all been updated. Well fucking done.'
		flash[:type] = 'success'
		redirect_to root_url
	end
end