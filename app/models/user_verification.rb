class UserVerification < ActiveRecord::Base
	attr_accessible :has_verified, :identifier, :user_id

	belongs_to :user

	before_create :create_identifier

	def self.verify(verification_id)
		# Check shit exists
		verification = find_by_identifier(verification_id)

		# Check hasn't been used
		return '1' if verification.has_verified

		# Check hasn't expired
		return '2' if verification.created_at + 1.hour < DateTime.now

		# Mark as used
		verification.has_verified = true
		verification.save!

		# Set user as normal, not validating
		verification.user.role_id = Role.find_by_identifier(2).id
		verification.user.save()

		return true
	end

	def generate_url
		return "http://branchapp.co/user/verify/#{self.identifier}"
	end

	private
		def create_identifier
			self.identifier = SecureRandom.uuid()
		end
end
