module ApplicationHelper
	def title(page_title)
		content_for :title, 'Branch' + page_title.to_s
	end

	def container_type(container_type)
		content_for :container_type, container_type.to_s
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


	# halo 4 sidebar
	def h4_gamertag(h4_gamertag)
		content_for :h4_gamertag, h4_gamertag.to_s
	end
	def h4_sidebar_overview(h4_sidebar_overview)
		content_for :h4_sidebar_overview, h4_sidebar_overview.to_s
	end
	def h4_sidebar_game_history(h4_sidebar_game_history)
		content_for :h4_sidebar_game_history, h4_sidebar_game_history.to_s
	end
	def h4_sidebar_csr(h4_sidebar_csr)
		content_for :h4_sidebar_csr, h4_sidebar_csr.to_s
	end
	def h4_sidebar_commendations(h4_sidebar_commendations)
		content_for :h4_sidebar_commendations, h4_sidebar_commendations.to_s
	end
	def h4_sidebar_specializations(h4_sidebar_specializations)
		content_for :h4_sidebar_specializations, h4_sidebar_specializations.to_s
	end
	def h4_sidebar_summary(h4_sidebar_summary)
		content_for :h4_sidebar_summary, h4_sidebar_summary.to_s
	end
	def h4_sidebar_by_playlist(h4_sidebar_by_playlist)
		content_for :h4_sidebar_by_playlist, h4_sidebar_by_playlist.to_s
	end
	def h4_sidebar_by_map(h4_sidebar_by_map)
		content_for :h4_sidebar_by_map, h4_sidebar_by_map.to_s
	end
	def h4_sidebar_medals(h4_sidebar_medals)
		content_for :h4_sidebar_medals, h4_sidebar_medals.to_s
	end
	def h4_sidebar_weapons(h4_sidebar_weapons)
		content_for :h4_sidebar_weapons, h4_sidebar_weapons.to_s
	end
	def h4_sidebar_enemies(h4_sidebar_enemies)
		content_for :h4_sidebar_enemies, h4_sidebar_enemies.to_s
	end


	# leaf
	def gamertag_to_safe_leaf(gamertag)
		return gamertag.downcase.tr(" ", "_").strip
	end

	# errorz
	def setup_error_notification(type, message)
		flash[:type] = type
		flash[:notice] = message
	end
end
