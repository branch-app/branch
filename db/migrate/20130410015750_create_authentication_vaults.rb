class CreateAuthenticationVaults < ActiveRecord::Migration
  def change
    create_table :authentication_vaults do |t|
      t.text :wlid_access_token
      t.text :wlid_authentication_token
      t.datetime :wlid_expire
      t.text :spartan_token

      t.timestamps
    end
  end
end
