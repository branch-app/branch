Rails.application.routes.draw do
	namespace :xbox_live, :path => 'xbox-live' do
		get '/', to: 'home#index'
		get '/:gamertag', to: 'profile#profile'
	end

	namespace :halo4, :path => "halo-4" do
		get '/', to: 'home#index'
		get '/:gamertag', to: 'service_record#service_record'
	end

	get '/', to: 'home#index'
	get '/search', to: 'search#index'
end
