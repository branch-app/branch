scheduler = Rufus::Scheduler.start_new

=begin
	scheduler.in '20m' do
		puts 'order ristretto'
	end

	scheduler.at 'Thu Mar 26 07:31:43 +0900 2009' do
		puts 'order pizza'
	end

	scheduler.cron '0 22 * * 1-5' do
		# every day of the week at 22:00 (10pm)
		puts 'activate security system'
	end

	scheduler.every '1m' do
		# do shit
	end
=end

	# re-authenticate windows live and spartan tokens
	scheduler.every '55m' do
		BackgroundAuthController.UpdateAuthentication
	end

	# re-cache services list (I don't think they change this much, so lets do it once a week. We can always manually re-cache it from the acp later)
	scheduler.every '1w' do
		X343ApiController.UpdateServicesList
	end

	# re-cache playlists and population
	scheduler.every '1h' do
		X343ApiController.UpdatePlaylists
	end


	# re-cache the challenges
	scheduler.cron '5 10 * * 1-7' do
		X343ApiController.UpdateChallenges
	end