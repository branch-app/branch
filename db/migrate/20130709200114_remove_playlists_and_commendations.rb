class RemovePlaylistsAndCommendations < ActiveRecord::Migration
	def up
		drop_table :h4_playlists
		drop_table :h4_global_challenges
	end

	def down
		create_table "h4_playlists", :force => true do |t|
			t.datetime "created_at",                       :null => false
			t.datetime "updated_at",                       :null => false
			t.text     "data",       :limit => 2147483647
		end

		create_table "h4_player_challenges", :force => true do |t|
			t.text     "data",       :limit => 2147483647
			t.string   "gamertag"
			t.datetime "created_at",                       :null => false
			t.datetime "updated_at",                       :null => false
		end
	end
end
