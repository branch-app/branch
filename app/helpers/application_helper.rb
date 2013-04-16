module ApplicationHelper
	def title(page_title)
		content_for :title, 'Branch' + page_title.to_s
	end

	def head_style(header_style)
		content_for :head_style, header_style.to_s
	end

	def active_home(active_home)
		content_for :active_home, active_home.to_s
	end
	def active_halo4(active_halo4)
		content_for :active_halo4, active_halo4.to_s
	end
	def active_blog(active_blog)
		content_for :active_blog, active_blog.to_s
	end
	def active_about(active_about)
		content_for :active_about, active_about.to_s
	end

	def search_query(search_query)
		content_for :search_query, search_query.to_s
	end

	def self.SetupErrorNotification(type, message)
		flash[:type] = type
		flash[:notice] = message
	end

	class Numeric
		def percent_of(n)
			self.to_f / n.to_f * 100.0
		end
	end
end
