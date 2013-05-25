class CreateBlogPosts < ActiveRecord::Migration
  def change
    create_table :blog_posts do |t|
      t.string :title
      t.string :title_safe
      t.integer :author_id
      t.text :body
      t.string :summary
      t.boolean :is_published
      t.string :tags

      t.timestamps
    end
  end
end
