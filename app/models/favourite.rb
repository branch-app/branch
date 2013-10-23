class Favourite < ActiveRecord::Base
  attr_accessible :user_id

  has_one :h4_favourite
end
