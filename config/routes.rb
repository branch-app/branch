Rails.application.routes.draw do
  namespace :halo4 do
    get '/halo-4/', to: 'home#index'
  end

  get '/', to: 'home#index'
end
