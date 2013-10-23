class H4Favourite < ActiveRecord::Base
  attr_accessible :game_id, :favourite_id, :game_variant_id, :game_variant_name, :map_id, :map_variant_name, :mvp_gamertag, :mvp_kd, :mvp_kills

  belongs_to :favourite
end
