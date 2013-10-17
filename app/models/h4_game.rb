class H4Game < ActiveRecord::Base
	attr_accessible :game_id, :h4_service_record
	
	belongs_to :h4_service_record
end
