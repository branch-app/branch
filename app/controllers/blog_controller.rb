class BlogController < ApplicationController
	def index 
		blog_entries = BlogPost.find(:all)
		blog_entry_count = BlogPost.count

		render json: { entries: blog_entries, count: blog_entry_count }
	end

	def view 
		title_safe = params[:title_safe]
		year = params[:year]
		month = params[:month]
		day = params[:day]

		@blog_entry = BlogPost.find_by_title_safe title_safe
	end
end