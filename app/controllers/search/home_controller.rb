class Search::HomeController < ApplicationController
	before_filter :get_query

	@query = nil
	def get_query
		@query = params['q'].strip()

		# check search query
		if (@query == nil || @query == '')
			set_flash_message('warning', 'Eh...', "We can't search things from an empty search query.")
			redirect_to(root_url())
			return
		end

		session[:search_query] = @query
	end

	def index
		@branch_accounts = get_branch_results()
		@halo4_accounts = get_halo4_results()
	end

	private
		def get_branch_results(start_index = 0, count = 12)
			return User.where('username like ?', "%#{@query}%").offset(start_index).limit(count)
		end

		def get_halo4_results(start_index = 0, count = 12)
			halo4_accounts = H4ServiceRecord.joins('JOIN gamertags ON h4_service_records.gamertag_id = gamertags.id').where('gamertags.gamertag like ?', "%#{@query}%").offset(start_index).limit(count)

			# check halo 4 has exact match
			has_exact = false
			halo4_accounts.map{ |g| has_exact = true if (g.gamertag.gamertag.downcase() == @query.downcase()) }

			if (!has_exact) 
				H4Api.get_player_service_record(@query)
				halo4_accounts = H4ServiceRecord.joins('JOIN gamertags ON h4_service_records.gamertag_id = gamertags.id').where('gamertags.gamertag like ?', "%#{@query}%").offset(start_index).limit(count)
			end

			return halo4_accounts
		end
end