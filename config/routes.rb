BranchApp::Application.routes.draw do

	root :to => 'application#index'

	# dev shiz
	if Rails.env.development?
		get '/devcacheupdate' => 'update_caches#devcacheupdate'
	end

	# search
	match '/search/:gamertag/' => 'search#index'

	# halo4
	match '/halo4/challenges/' => 'halo4#challenges'
	match '/halo4/playlists/' => 'halo4#playlists'
	match '/halo4/servicerecord/:gamertag/' => 'halo4#service_record'
	match '/halo4/servicerecord/:gamertag/match-history/' => 'halo4#match_history'
	match '/halo4/servicerecord/:gamertag/match-history/:game_mode/' => 'halo4#match_history'
	match '/halo4/servicerecord/:gamertag/match-history/:game_mode/:page/' => 'halo4#match_history'
	match '/halo4/servicerecord/:gamertag/match/:matchid/' => 'halo4#view_match'

	# halo4 cut
	## match '/halo4/challenges/:gamertag/' => 'halo4#challenges'

  resources :application

  # The priority is based upon order of creation:
  # first created -> highest priority.

  # Sample of regular route:
  #   match 'products/:id' => 'catalog#view'
  # Keep in mind you can assign values other than :controller and :action

  # Sample of named route:
  #   match 'products/:id/purchase' => 'catalog#purchase', :as => :purchase
  # This route can be invoked with purchase_url(:id => product.id)

  # Sample resource route (maps HTTP verbs to controller actions automatically):
  #   resources :products

  # Sample resource route with options:
  #   resources :products do
  #     member do
  #       get 'short'
  #       post 'toggle'
  #     end
  #
  #     collection do
  #       get 'sold'
  #     end
  #   end

  # Sample resource route with sub-resources:
  #   resources :products do
  #     resources :comments, :sales
  #     resource :seller
  #   end

  # Sample resource route with more complex sub-resources
  #   resources :products do
  #     resources :comments
  #     resources :sales do
  #       get 'recent', :on => :collection
  #     end
  #   end

  # Sample resource route within a namespace:
  #   namespace :admin do
  #     # Directs /admin/products/* to Admin::ProductsController
  #     # (app/controllers/admin/products_controller.rb)
  #     resources :products
  #   end

  # You can have the root of your site routed with "root"
  # just remember to delete public/index.html.
  # root :to => 'welcome#index'

  # See how all your routes lay out with "rake routes"

  # This is a legacy wild controller route that's not recommended for RESTful applications.
  # Note: This route will make all actions in every controller accessible via GET requests.
  # match ':controller(/:action(/:id))(.:format)'
end
