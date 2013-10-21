BranchApp::Application.routes.draw do

	# Core
	root :to => 'home#index'
	get '/welcome' => 'home#welcome', as: :home_welcome

	# Admin
	get '/domain/' => 'admin/home#index', as: :admin_home

	# User
	get '/user/signin' => 'user/session#new', as: :user_signin
	post '/user/signin' => 'user/session#create', as: :session_create
	get '/user/register' => 'user/user#new', as: :user_register
	post '/user/register' => 'user/user#create', as: :user_create
	get '/user/signout' => 'user/session#destroy', as: :user_signout
	get '/user/verify/:verification_id' => 'user/user#verify', as: :user_verify
	get '/user/resend_verification' => 'user/user#resend_verification', as: :user_resend_verification
	match '/user/follow' => 'user/user#follow', as: :user_follow, via: [ :post, :delete ]

	# Account
	match '/user/:id/' => 'user/account/home#index', via: [ :get ], as: :user_view, constraints: UserConstraint
	get '/user/:id/following' => 'user/account/following#index', as: :user_following, constraints: UserConstraint
	get '/user/:id/followers' => 'user/account/followers#index', as: :user_followers, constraints: UserConstraint
	get '/user/:id/favourites' => 'user/account/favourites#index', as: :user_favourites, constraints: UserConstraint

	# Account Settings
	get '/settings' => 'user/settings/profile#index'
	get '/settings/profile' => 'user/settings/profile#index', as: :settings_profile
	post '/settings/profile' => 'user/settings/profile#update', as: :settings_profile_update
	get '/settings/sessions' => 'user/settings/session#index', as: :settings_session
	delete '/settings/sessions' => 'user/settings/session#destroy', as: :settings_session_destroy

	# Blog
	get '/blog(/:page(/:focus))' => 'blog/home#index', as: :blog_home
	get '/blog/:year/:month/:day/:slug' => 'blog/view#index', as: :blog_view

	# About
	get '/about' => 'about/home#index', as: :about_home

	# Legal
	get '/legal/privacy' => 'legal/privacy#index', as: :legal_privacy
	get '/legal/terms' => 'legal/terms#index', as: :legal_terms

	# Search
	get '/search/all/' => 'search/home#index', as: :search_query
	get '/search/branch/:page/' => 'search/focus#branch', as: :search_branch_query
	get '/search/halo4/:page/' => 'search/focus#halo4', as: :search_halo4_query

	# Halo 4
	get '/halo4/' => 'halo4/home#index', as: :halo4_home
	get '/halo4/servicerecord/:gamertag/' => 'halo4/service_record#index', as: :halo4_servicerecord
	get '/halo4/servicerecord/:gamertag/match-history' => 'halo4/game_history#index'
	get '/halo4/servicerecord/:gamertag/game-history(/:sub_view/:page)' => 'halo4/game_history#index', as: :halo4_gamehistory
	get '/halo4/servicerecord/:gamertag/match/:game_id' => 'halo4/game_history#view'
	get '/halo4/servicerecord/:gamertag/game/:game_id' => 'halo4/game_history#view', as: :halo4_viewgame
	get '/halo4/servicerecord/:gamertag/competitive-skill-rank' => 'halo4/competitive_skill_rank#index'
	get '/halo4/servicerecord/:gamertag/csr' => 'halo4/competitive_skill_rank#index', as: :halo4_csr
	get '/halo4/servicerecord/:gamertag/csr/:id-:slug' => 'halo4/competitive_skill_rank#details', as: :halo4_csr_details
	get '/halo4/servicerecord/:gamertag/competitive-skill-rank/:id' => 'halo4/competitive_skill_rank#details'
	get '/halo4/servicerecord/:gamertag/commendations(/:sub_view)' => 'halo4/commendations#index', as: :halo4_commendations
	get '/halo4/servicerecord/:gamertag/specializations' => 'halo4/specialization#index', as: :halo4_specialization

end
