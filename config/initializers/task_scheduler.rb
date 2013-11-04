scheduler = Rufus::Scheduler.new
	include I343Auth
	include H4Api

	# re-authenticate windows live and spartan tokens
	scheduler.every '45m' do
		if (Rails.env.development?)
			I343Auth.update_authentication()
		end
	end
