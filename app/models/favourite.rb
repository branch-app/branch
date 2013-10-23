class Favourite < ActiveRecord::Base
  attr_accessible :user_id

  has_one :h4_favourite
  belongs_to :user

  def self.find_halo_4_favourites_by_user_id(user_id)
  	favourites = find_all_by_user_id(user_id)
  	if (favourites == nil)
  		return [ ]
  	end

  	h4_favs = [ ]
  	favourites.each do |favourite|
  		if (favourite.h4_favourite != nil)
  			h4_favs << favourite
  		end
  	end

  	return h4_favs
  end
end
