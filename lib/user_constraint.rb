module UserConstraint
	def self.matches?(request)
		User.exists?(username: request.params[:id])
	end
end
