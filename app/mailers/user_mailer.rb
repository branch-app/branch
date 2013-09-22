class UserMailer < ActionMailer::Base
	default from: "Branch <no-reply@branchapp.co>"

	def validation_email(user, verification)
		@user = user
		@verification = verification
		mail(to: user.email, subject: "Welcome to Branch, #{user.username}")
	end
end
