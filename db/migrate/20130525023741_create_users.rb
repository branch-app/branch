class CreateUsers < ActiveRecord::Migration
  def change
    create_table :users do |t|
      t.string :username
      t.string :email
      t.string :gamertag
      t.string :password_crypted
      t.string :name
      t.boolean :is_admin

      t.timestamps
    end
  end
end
