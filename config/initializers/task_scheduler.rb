scheduler = Rufus::Scheduler.start_new
	include I343Auth
	include H4Api

	# re-authenticate windows live and spartan tokens
	scheduler.every '45m' do
		I343Auth.update_authentication()
	end
