class H4GameHistory < ActiveRecord::Base
	attr_accessible :chapter_id, :count, :h4_service_record_id, :mode_id, :start_index

	belongs_to :h4_service_record
end
