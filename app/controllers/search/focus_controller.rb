class Search::FocusController < Search::HomeController
	before_filter :get_page
	@page = 1
	PER_PAGE = 18
	def get_page
		@page = params[:page].to_i
	end

	def branch
		if (@page == nil || @page < 1)
			redirect_to(search_branch_query_path(q: @query, page: 1))
			return
		end

		start_index = PER_PAGE * (@page - 1)

		@branch_accounts = get_branch_results(start_index, PER_PAGE + 1)
		@has_less = (@page > 1)
		if (@branch_accounts.length == PER_PAGE + 1)
			@has_more = true
			@branch_accounts.pop
		end

		@previous_page = @page - 1
		@previous_page = @page if (!@has_less)
		@next_page = @page + 1
		@next_page = @page if (!@has_more)
	end

	def halo4
		if (@page == nil || @page < 1)
			redirect_to(search_halo4_query_path(q: @query, page: 1))
			return
		end

		start_index = PER_PAGE * (@page - 1)

		@halo4_accounts = get_halo4_results(start_index, PER_PAGE + 1)
		@has_less = (@page > 1)
		if (@halo4_accounts.length == PER_PAGE + 1)
			@has_more = true
			@halo4_accounts.pop
		end

		@previous_page = @page - 1
		@previous_page = @page if (!@has_less)
		@next_page = @page + 1
		@next_page = @page if (!@has_more)
	end
end
