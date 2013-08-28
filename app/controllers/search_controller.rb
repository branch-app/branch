class SearchController < ApplicationController

	def search
		@focus_target = params['focus'].downcase
		@query = params['q']

		# check search query
		if @query.strip == nil || @query.strip == ''
			flash[:danger] = "Empty Search Query"
			redirect_to root_url
			return
		end

		# check focus target
		unless @focus_target == 'all' || @focus_target == 'h4' || @focus_target == 'ba'
			flash[:danger] = "You can't focus on that type of element"
			redirect_to root_url
			return
		end

		@focus_friendly = 'Halo 4 Gamertags' if @focus_target == 'h4'
		@focus_friendly = 'Branch Accounts' if @focus_target == 'ba'
	end

end
