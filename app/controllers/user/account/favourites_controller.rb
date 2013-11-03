class User::Account::FavouritesController < User::Account::HomeController
	PER_PAGE = 15

	def index
		@page = params[:page].to_i
		if (@page == nil || @page < 1)
			redirect_to(user_favourites_path(username: @username, page: 1))
			return
		end

		start_index = PER_PAGE * (@page - 1)

		@favourites = @account.favourite.order('created_at DESC').offset(start_index).limit(PER_PAGE + 1)
		@has_less = (@page > 1)
		if (@favourites.length == PER_PAGE + 1)
			@has_more = true
			@favourites.pop
		end

		@previous_page = @page - 1
		@previous_page = @page if (!@has_less)
		@next_page = @page + 1
		@next_page = @page if (!@has_more)
	end
end
