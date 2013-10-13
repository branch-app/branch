class CreateH4ServiceRecords < ActiveRecord::Migration
  def change
    create_table :h4_service_records do |t|
      t.integer :gamertag_id

      t.timestamps
    end
  end
end
