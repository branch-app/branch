BranchApp::Application.routes.draw do

	root :to => 'application#index'

	# about
	match '/about/' => 'application#about'

	# users
	get '/login' => 'sessions#new'
	get '/logout' => 'sessions#destroy'
	get '/register' => 'users#new'

	# search
	get '/search/' => 'search#index'

	# blog
	match '/blog/' => 'blog#index'
	match '/blog/:year/:month/:day/:title_safe' => 'blog#view'

	# halo4
	match '/halo4/challenges/' => 'halo4#challenges'
	match '/halo4/playlists/' => 'halo4#playlists'
	match '/halo4/servicerecord/:gamertag/' => 'halo4#service_record'
	match '/halo4/servicerecord/:gamertag/match-history/' => 'halo4#match_history'
	match '/halo4/servicerecord/:gamertag/match-history/:game_mode/' => 'halo4#match_history'
	match '/halo4/servicerecord/:gamertag/match-history/:game_mode/:page/' => 'halo4#match_history'
	match '/halo4/servicerecord/:gamertag/match/:matchid/' => 'halo4#view_match'
	match '/halo4/servicerecord/:gamertag/competitive-skill-rank/' => 'halo4#competitive_skill_rank'
	match '/halo4/servicerecord/:gamertag/competitive-skill-rank/:playlist_id/' => 'halo4#unique_playlist_csr'
	match '/halo4/servicerecord/:gamertag/commendations/' => 'halo4#commendations'
	match '/halo4/servicerecord/:gamertag/specializations/' => 'halo4#specializations'
	match '/halo4/servicerecord/:gamertag/summary/' => 'halo4#summary'

	# halo4 cut
	## match '/halo4/challenges/:gamertag/' => 'halo4#challenges'

	# dev shiz
	get '/devcacheupdate' => 'update_caches#devcacheupdate' if Rails.env.development?

	resources :application
end
