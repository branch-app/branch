BranchApp::Application.routes.draw do
  root :to => 'home#welcome'

  get '/halo4/' => 'halo4/home#index', as: :halo4_home
end
