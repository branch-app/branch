class CreateH4Games < ActiveRecord::Migration
  def change
    create_table :h4_games do |t|
      t.integer :h4_service_record_id
      t.string :game_id

      t.timestamps
    end
  end
end
