class CreateGamertags < ActiveRecord::Migration
  def change
    create_table :gamertags do |t|
      t.string :gamertag

      t.timestamps
    end
  end
end
