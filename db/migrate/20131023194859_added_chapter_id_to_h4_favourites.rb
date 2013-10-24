class AddedChapterIdToH4Favourites < ActiveRecord::Migration
  def change
  	add_column(:h4_favourites, :chapter_id, :integer)
  end
end
