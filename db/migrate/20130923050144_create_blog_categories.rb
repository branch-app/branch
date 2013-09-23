class CreateBlogCategories < ActiveRecord::Migration
  def change
    create_table :blog_categories do |t|
      t.string :name
      t.string :description
      t.string :slug

      t.timestamps
    end
  end
end
