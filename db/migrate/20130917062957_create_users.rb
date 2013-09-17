class CreateUsers < ActiveRecord::Migration
  def change
    create_table :users do |t|
      t.string :username
      t.string :name
      t.string :email
      t.string :password
      t.integer :gamertag_id
      t.integer :role_id

      t.timestamps
    end
  end
end
