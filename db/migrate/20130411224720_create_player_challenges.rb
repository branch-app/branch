class CreatePlayerChallenges < ActiveRecord::Migration
  def change
    create_table :player_challenges do |t|
      t.text :data, :limit => 2147483647
      t.string :gamertag

      t.timestamps
    end
  end
end
