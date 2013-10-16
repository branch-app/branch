class String
	def to_slug
		# Strip the string
		ret = self.strip().downcase()

		# Blow away apostrophes
		ret.gsub!(/['`]/,"")

		# @ --> at, and & --> and
		ret.gsub!(/\s*@\s*/, " at ")
		ret.gsub!(/\s*&\s*/, " and ")

		# Replace all non alphanumeric, underscore or periods with underscore
		ret.gsub!(/\s*[^A-Za-z0-9\.\-]\s*/, '_')

		# Convert double underscores to single
		ret.gsub!(/_+/,"_")

		# Strip off leading/trailing underscore
		ret.gsub!(/\A[_\.]+|[_\.]+\z/,"")

		return ret
	end
end