class CreatePlaylists < ActiveRecord::Migration
  def change
    create_table :playlists do |t|
      t.integer :playlist_id
      t.boolean :is_current
      t.string :name
      t.string :description
      t.integer :mode_id
      t.string :mode_name
      t.integer :max_party_size
      t.integer :max_local_players
      t.boolean :is_free_for_all
      t.string :playlist_icon
      t.text :game_varients
      t.text :map_varients

      t.timestamps
    end
  end
end
