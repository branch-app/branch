class CreateH4Favourites < ActiveRecord::Migration
  def change
    create_table :h4_favourites do |t|
      t.integer  :favourite_id
      t.string   :game_id
      t.string   :map_variant_name
      t.string   :game_variant_name
      t.integer  :game_variant_id
      t.integer  :map_id
      t.string   :mvp_gamertag
      t.integer  :mvp_kills
      t.integer  :mvp_kd

      t.timestamps
    end
  end
end
