BranchApp::Application.routes.draw do

  # Core
  root :to => 'home#welcome'
  get '/search/:focus/' => 'search#search', as: :search_query, defaults: { focus: 'all' }

  # Halo 4
  get '/halo4/' => 'halo4/home#index', as: :halo4_home
  get '/halo4/servicerecord/:gamertag/' => 'halo4/service_record#index', as: :halo4_servicerecord
end
