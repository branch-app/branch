class CreateGamertagReplacements < ActiveRecord::Migration
  def change
    create_table :gamertag_replacements do |t|
      t.string :target
      t.string :replacement

      t.timestamps
    end
  end
end
