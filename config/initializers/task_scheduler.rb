scheduler = Rufus::Scheduler.new
	include I343Auth
	include H4Api

	# Get fresh Spartan Tokens
	scheduler.every('45m') do
		if (Rails.env.development?)
			I343Auth.update_authentication()
		end
	end

	# Updated Services List
	scheduler.every('1h') do
		I343Auth.update_services_list()
	end
