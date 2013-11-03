module User::UserHelper
	def has_validation_errors(errors, type)
		return 'display: none;' if (errors.messages[type].length == 0)
	end
end
