class Blog::HomeController < ApplicationController
	def index
		@page = params[:page].to_i
		if @page == nil || @page < 1
			redirect_to(blog_home_path(page: 1))
			return
		end

		@blog_posts = BlogPost.where('is_published = TRUE').order('created_at DESC').limit(6)

		@has_less = (@page > 1)
		@has_more = (@blog_posts.length == 6)

		@previous_page = @page - 1
		@previous_page = @page if !@has_less
		@next_page = @page + 1
		@next_page = @page if !@has_more

		# render json: { has_less: @has_less, has_more: @has_more }
	end
end