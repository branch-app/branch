class AddSlugColumnToBlogPosts < ActiveRecord::Migration
  def change
  	add_column(:blog_posts, :slug, :string)
  end
end
