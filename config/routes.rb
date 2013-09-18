BranchApp::Application.routes.draw do
  
  # Core
  root :to => 'home#index'
  get '/welcome' => 'home#welcome', as: :home_welcome

  # User
  get '/user/signin' => 'user/session#new', as: :user_signin
  get '/user/register' => 'user/user#new', as: :user_register

  # Account
  get '/user/:username' => 'user/account/home#index', as: :user_view
  get '/user/:username/friends' => 'user/account/friends#index', as: :user_friends
  get '/user/:username/favourites' => 'user/account/favourites#index', as: :user_favourites

  # Search
  get '/search/:focus/' => 'search#search', as: :search_query, defaults: { focus: 'all' }

  # Halo 4
  get '/halo4/' => 'halo4/home#index', as: :halo4_home
  get '/halo4/servicerecord/:gamertag/' => 'halo4/service_record#index', as: :halo4_servicerecord
  get '/halo4/servicerecord/:gamertag/match-history' => 'halo4/game_history#index'
  get '/halo4/servicerecord/:gamertag/game-history(/:sub_view/:page)' => 'halo4/game_history#index', as: :halo4_gamehistory
  get '/halo4/servicerecord/:gamertag/match/:game_id' => 'halo4/game_history#view'
  get '/halo4/servicerecord/:gamertag/game/:game_id' => 'halo4/game_history#view', as: :halo4_viewgame
  get '/halo4/servicerecord/:gamertag/competitive-skill-rank' => 'halo4/competitive_skill_rank#index'
  get '/halo4/servicerecord/:gamertag/csr' => 'halo4/competitive_skill_rank#index', as: :halo4_csr
  get '/halo4/servicerecord/:gamertag/csr/:id' => 'halo4/competitive_skill_rank#details', as: :halo4_csr_details
  get '/halo4/servicerecord/:gamertag/competitive-skill-rank/:id' => 'halo4/competitive_skill_rank#details'
  get '/halo4/servicerecord/:gamertag/commendations(/:sub_view)' => 'halo4/commendations#index', as: :halo4_commendations
  get '/halo4/servicerecord/:gamertag/specializations' => 'halo4/specialization#index', as: :halo4_specialization

end
