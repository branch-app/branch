class RemoveDataFromMatch < ActiveRecord::Migration
	def up
		remove_column :h4_player_matches, :data
	end

	def down
		add_column :h4_player_matches, :data, :text, :limit => 2147483647
	end
end
