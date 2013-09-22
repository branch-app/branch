module UserConstraint
	def self.matches?(request)
		User.exists?(username: request.params[:username])
	end
end
