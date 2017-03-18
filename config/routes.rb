Rails.application.routes.draw do
	namespace :xbox_live, :path => 'xbox-live' do
		get '/', to: 'home#index'
		get '/:gamertag', to: 'profile#profile'
	end

	namespace :halo4, :path => "halo-4" do
		get '/', to: 'home#index'
		get '/:gamertag', to: 'service_record#index'
		get '/:gamertag/matches/', to: 'matches#recent-match'
		get '/:gamertag/matches/:id', to: 'matches#match', constraints: { id: /[a-f0-9]{16}/ }
		get '/:gamertag/matches/:slug', to: 'matches#recent_matches'
	end

	get '/', to: 'home#index'
	get '/search', to: 'search#index'
end
