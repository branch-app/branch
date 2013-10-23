class AddModeIdToH4Favourites < ActiveRecord::Migration
  def change
  	add_column(:h4_favourites, :mode_id, :integer)
  end
end
