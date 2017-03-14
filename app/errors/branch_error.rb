class BranchError < StandardError
	attr_reader :code, :reasons, :metadata

	def initialize(code, reasons = nil, metadata = nil)
		@code = code
		@reaspons = reasons
		@metadata = metadata

		super(@code)
	end

	def self.coerce(json)
		self.new(json.code, json.reasons, json.metadata)
	end
end
