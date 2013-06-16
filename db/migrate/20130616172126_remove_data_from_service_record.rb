class RemoveDataFromServiceRecord < ActiveRecord::Migration
	def up
		remove_column :h4_player_servicerecords, :data
	end

	def down
		add_column :h4_player_servicerecords, :data, :text, :limit => 2147483647
	end
end
