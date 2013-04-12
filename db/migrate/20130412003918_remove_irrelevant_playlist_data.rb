class RemoveIrrelevantPlaylistData < ActiveRecord::Migration
  def up
		remove_column :playlists, :playlist_id
		remove_column :playlists, :is_current
		remove_column :playlists, :name
		remove_column :playlists, :description
		remove_column :playlists, :mode_id
		remove_column :playlists, :mode_name
		remove_column :playlists, :max_party_size
		remove_column :playlists, :max_local_players
		remove_column :playlists, :is_free_for_all
		remove_column :playlists, :playlist_icon
		remove_column :playlists, :game_variants
		remove_column :playlists, :map_variants
		remove_column :playlists, :population_count

		add_column :playlists, :data, :text, :limit => 2147483647
  end

  def down
	  remove_column :playlists, :playlist_id
	  remove_column :playlists, :is_current
	  remove_column :playlists, :name
	  remove_column :playlists, :description
	  remove_column :playlists, :mode_id
	  remove_column :playlists, :mode_name
	  remove_column :playlists, :max_party_size
	  remove_column :playlists, :max_local_players
	  remove_column :playlists, :is_free_for_all
	  remove_column :playlists, :playlist_icon
	  remove_column :playlists, :game_variants
	  remove_column :playlists, :map_variants
	  remove_column :playlists, :population_count

	  add_column :playlists, :data, :text, :limit => 2147483647
  end
end
