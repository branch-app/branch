class H4PlayerCommendation < ActiveRecord::Base
	attr_accessible :h4_service_record_id

	belongs_to :h4_service_record
end
