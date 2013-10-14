class CreateH4PlaylistOrientations < ActiveRecord::Migration
  def change
    create_table :h4_playlist_orientations do |t|
      t.integer :playlist_id
      t.boolean :is_team
      t.boolean :is_individual

      t.timestamps
    end
  end
end
