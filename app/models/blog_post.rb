class BlogPost < ActiveRecord::Base
  attr_accessible :author_id, :body, :is_published, :summary, :tags, :title, :title_safe
end
