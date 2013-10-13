class CreateH4GameHistories < ActiveRecord::Migration
  def change
    create_table :h4_game_histories do |t|
      t.integer :h4_service_record_id
      t.integer :start_index
      t.integer :count
      t.integer :mode_id
      t.integer :chapter_id

      t.timestamps
    end
  end
end
