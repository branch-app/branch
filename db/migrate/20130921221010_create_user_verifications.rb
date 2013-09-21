class CreateUserVerifications < ActiveRecord::Migration
  def change
    create_table :user_verifications do |t|
      t.integer :user_id
      t.string :identifier
      t.boolean :has_verified

      t.timestamps
    end
  end
end
