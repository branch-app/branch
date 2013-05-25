class BlogController < ApplicationController
	def index 
		blog_entries = BlogPost.find(:all)
		blog_entry_count = BlogPost.count

		render json: { entries: blog_entries, count: blog_entry_count }
	end

	def view 
	end
end