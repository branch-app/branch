class RemoveDataFromCommendations < ActiveRecord::Migration
	def up
		remove_column :h4_player_commendations, :data
	end

	def down
		add_column :h4_player_commendations, :data, :text, :limit => 2147483647
	end
end
