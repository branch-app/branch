class CreateSessions < ActiveRecord::Migration
  def change
    create_table :sessions do |t|
      t.string :identifier
      t.integer :user_id
      t.boolean :expired
      t.string :owner_ip
      t.string :location
      t.string :user_agent
      t.datetime :expires_at

      t.timestamps
    end
  end
end
