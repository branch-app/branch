class H4FavsFloatThatShit < ActiveRecord::Migration
  def up
  	change_column :h4_favourites, :mvp_kd, :decimal, precision: 10, scale: 2
  end

  def down
  	change_column :h4_favourites, :mvp_kd, :integer
  end
end
