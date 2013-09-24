class CreateBlogPosts < ActiveRecord::Migration
  def change
    create_table :blog_posts do |t|
      t.string :name
      t.string :short_body
      t.text :full_body
      t.integer :user_id
      t.boolean :is_published
      t.integer :blog_category_id

      t.timestamps
    end
  end
end
