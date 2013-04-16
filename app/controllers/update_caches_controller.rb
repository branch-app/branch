class UpdateCachesController < ApplicationController
	def devcacheupdate
		# authorize
		BackgroundAuthController.UpdateAuthentication

		# call
		X343ApiController.UpdateServicesList
		X343ApiController.UpdateMetaData
		X343ApiController.UpdateChallenges
		X343ApiController.UpdatePlaylists
	end
end
