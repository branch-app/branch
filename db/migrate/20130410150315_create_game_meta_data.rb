class CreateGameMetaData < ActiveRecord::Migration
  def change
    create_table :game_meta_data do |t|
      t.text :data

      t.timestamps
    end
  end
end
