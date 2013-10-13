class H4ServiceRecord < ActiveRecord::Base
	attr_accessible :gamertag_id

	belongs_to :gamertag

	def self.find_by_gamertag(gamertag)
		gamertag = Gamertag.find_by_gamertag(gamertag)
		return nil if !gamertag
		return find_by_gamertag_id(gamertag.id)
	end
end
