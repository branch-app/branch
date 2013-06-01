class BlogController < ApplicationController
	def index 
		@count_modifier = 5
		@page = params[:page].to_i
		@page = 1 if @page == 0 || @page < 0

		@blog_entries = BlogPost.where("id >= #{@count_modifier * (@page - 1)} AND is_published = 1").order('id ASC').limit(@count_modifier)
		@blog_entry_count = BlogPost.count

		# pagination calculation
		@lower = [ ]
		if @page == 1
			@lower = nil 
		else
			storage = @page - 1
			while storage > 0
				break if @lower.length > 1
				@lower << storage
				storage -= 1
			end
			@lower = @lower.reverse
		end

		@upper = [ ]
		if (@page * @count_modifier) >= @blog_entry_count
			@upper = nil 
		else
			storage = @page + 1
			while storage < @page + 3
				break if storage > (@blog_entry_count.to_f / @count_modifier.to_f).to_f.ceil
				@upper << storage
				storage += 1
			end
		end
	end

	def view 
		title_safe = params[:title_safe]
		year = params[:year]
		month = params[:month]
		day = params[:day]

		@blog_entry = BlogPost.find_by_title_safe title_safe

		if @blog_entry == nil || !@blog_entry.is_published
			# throw 404
			raise ActionController::RoutingError.new('Not Found')
		end

		@markdown = Redcarpet::Markdown.new(Redcarpet::Render::HTML, :autolink => true, :space_after_headers => true)
	end
end
