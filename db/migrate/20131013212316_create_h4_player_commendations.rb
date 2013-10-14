class CreateH4PlayerCommendations < ActiveRecord::Migration
  def change
    create_table :h4_player_commendations do |t|
      t.integer :h4_service_record_id

      t.timestamps
    end
  end
end
