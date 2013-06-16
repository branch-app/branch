class RemoveMetaDataTable < ActiveRecord::Migration
	def up
		drop_table :h4_game_metadata
	end

	def down
		create_table "h4_game_metadata", :force => true do |t|
			t.text     "data",       :limit => 2147483647
			t.datetime "created_at",                       :null => false
			t.datetime "updated_at",                       :null => false
		end
	end
end
