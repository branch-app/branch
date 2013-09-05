module Halo4::GameHistoryHelper

	def update_pagination(current_page, direction)
		if direction == 'older'
			return current_page if current_page == 0
			return current_page - 1
		elsif direction == 'newer'
			return current_page + 1
		end
	end

end
