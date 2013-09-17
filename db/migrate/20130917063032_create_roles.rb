class CreateRoles < ActiveRecord::Migration
  def change
    create_table :roles do |t|
      t.string :name
      t.string :description
      t.string :colour
      t.integer :identifier

      t.timestamps
    end
  end
end
