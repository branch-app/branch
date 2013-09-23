class BlogPost < ActiveRecord::Base
  attr_accessible :blog_category_id, :slug, :full_body, :is_published, :name, :short_body, :user_id
end
