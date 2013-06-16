class RemoveDataFromRecentMatches < ActiveRecord::Migration
	def up
		remove_column :h4_player_recent_matches, :data
	end

	def down
		add_column :h4_player_recent_matches, :data, :text, :limit => 2147483647
	end
end
