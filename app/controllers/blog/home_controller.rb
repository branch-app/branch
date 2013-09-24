class Blog::HomeController < ApplicationController

	PER_PAGE = 3

	def index
		@page = params[:page].to_i
		@focus = BlogCategory.find_by_slug(params[:focus])

		if @page == nil || @page < 1
			redirect_to(blog_home_path(page: 1))
			return
		end
		if @focus == nil && params[:focus] != nil
			redirect_to(blog_home_path(page: 1))
			return
		end

		@blog_posts = [ ]
		if @focus
			@blog_posts = BlogPost.where('is_published = TRUE AND blog_category_id = ?', @focus.id).order('created_at DESC').limit(PER_PAGE).offset((@page - 1) * (PER_PAGE - 1)) 
		else
			@blog_posts = BlogPost.where('is_published = TRUE').order('created_at DESC').limit(PER_PAGE).offset((@page - 1) * (PER_PAGE - 1)) 
		end

		@has_less = (@page > 1)
		if (@blog_posts.length == PER_PAGE)
			@has_more = true
			@blog_posts.pop
		end

		@previous_page = @page - 1
		@previous_page = @page if !@has_less
		@next_page = @page + 1
		@next_page = @page if !@has_more

		# render json: { has_less: @has_less, has_more: @has_more }
	end
end