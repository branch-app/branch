class Blog::ViewController < Blog::HomeController
	def index
		valid_blog_posts = BlogPost.where('is_published = TRUE AND slug = ?', params[:slug])
		@blog_post = nil
		valid_blog_posts.each do |blog_post|
			if blog_post.created_at.year == params[:year].to_i && blog_post.created_at.month == params[:month].to_i && blog_post.created_at.day == params[:day].to_i
				@blog_post = blog_post
				break
			end
		end

		if @blog_post == nil
			redirect_to(blog_home_path)
			return
		end
		@markdown = Redcarpet::Markdown.new(Redcarpet::Render::HTML, :autolink => true, :space_after_headers => true)
	end
end