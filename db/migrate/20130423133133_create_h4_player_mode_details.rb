class CreateH4PlayerModeDetails < ActiveRecord::Migration
  def change
    create_table :h4_player_mode_details do |t|
      t.string :gamertag
      t.text :campaign_data,    :limit => 2147483647
      t.text :spartan_ops_data,    :limit => 2147483647
      t.text :war_games_data,    :limit => 2147483647
      t.text :custom_data,    :limit => 2147483647

      t.timestamps
    end
  end
end
