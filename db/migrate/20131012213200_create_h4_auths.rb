class CreateH4Auths < ActiveRecord::Migration
  def change
    create_table :h4_auths do |t|
      t.text :spartan_token
      t.datetime :expires_at

      t.timestamps
    end
  end
end
