class UpdatePlaylistMissingPopulation < ActiveRecord::Migration
  def up
		add_column :playlists, :population_count, :integer
  end

  def down
  end
end
