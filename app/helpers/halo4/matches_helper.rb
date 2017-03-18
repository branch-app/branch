module Halo4::MatchesHelper
	def humanize_mode(mode)
		case mode
			when 3
				return 'War Games'
			when 4
				return 'Campaign'
			when 5
				return 'Spartan Ops'
			when 6
				return 'Custom Games'
			end
	end

	def header_background_url(mode)
		case mode
			when 3, 6
				return '/images/games/halo4/H4MapAssets/large/ragnarok.jpg'
			when 4
				return '/images/games/halo4/H4MapAssets/large/forerunners.jpg'
			when 5
				return '/images/games/halo4/H4MapAssets/large/reclaimer.jpg'
		end
	end
end
