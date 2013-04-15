class H4PlayerRecentMatches < ActiveRecord::Base
  attr_accessible :gamertag, :data, :start_index, :count, :mode_id, :chapter_id
end
