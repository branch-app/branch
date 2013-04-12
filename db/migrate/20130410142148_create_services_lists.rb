class CreateServicesLists < ActiveRecord::Migration
  def change
    create_table :services_lists do |t|
      t.string :type
      t.string :name
      t.text :url

      t.timestamps
    end
  end
end
