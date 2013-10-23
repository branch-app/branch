class User < ActiveRecord::Base
	attr_accessible :email, :gamertag_id, :gamertag_friendly, :name, :password, :password_confirmation, :role_id, :username
	attr_accessor :password_confirmation, :gamertag_friendly

	has_many :session
	has_many :user_verification
	has_many :favourite
	has_many :following_follows, class_name: :Follow, foreign_key: :follower_id
	has_many :followers_follows, class_name: :Follow, foreign_key: :following_id
	has_many :following, class_name: :User, through: :following_follows
	has_many :followers, class_name: :User, through: :followers_follows
	belongs_to :role
	belongs_to :gamertag
	
	validates_presence_of :email, :gamertag_friendly, :name, :password
	validates_presence_of :password_confirmation, on: :create

	validates_length_of :password, if: ->(a) { a.password.present? }, minimum: 7
	validates_length_of :username, in: 1..30
	validates_length_of :name, in: 3..30

	validates_uniqueness_of :email, :username
	validates_uniqueness_of :email, :email

	validate :validate_password_confirmation, on: :create
	validate :map_gamertag
	validate :password_is_complex, if: ->(a) { a.password.present? }

	validates_format_of :email, with: /\A[\w\d\-.+]+@[\w\d\-.+]+\.[a-z]{2,25}\Z/i
	validates_format_of :username, with: /\A\w+\Z/
	validates_format_of :name, with: /\A[A-Za-z\- ]+\z/
	validates_format_of :gamertag_friendly, with: /\A[a-z]([a-z0-9]{0,15} ?)*\Z/i

	before_create :hash_password
	after_create :set_to_validating

	#-- Inner Functions --#
	def get_h4_service_record()
		return gamertag if (gamertag == nil)
		return gamertag.h4_service_record()
	end

	def following?(user)
		return following.include?(user)
	end

	def set_to_validating
		update_attribute(:role_id, Role.find_by_identifier(1).id)

		# send validation email
		verification = UserVerification.new(has_verified: false, user_id: self.id)
		verification.save!

		UserMailer.validation_email(self, verification).deliver
	end

	#-- Outer Functionns --#
	def self.authenticate(identifier, password)
		user = find_by_username(identifier)
		user = find_by_email(identifier) if user == nil

		return user if user == nil
		return user if Hashing.validate(password, user.password)
	end

	private
		def map_gamertag
			g_tag = Gamertag.find_by_gamertag(gamertag_friendly)
			if g_tag == nil
				g_tag = Gamertag.new(gamertag: gamertag_friendly)
				if g_tag.save
					self.gamertag_id = g_tag.id
				else
					errors.add(:gamertag, 'seems to be Invalid')
				end
			else
				self.gamertag_id = g_tag.id
			end
		end

		def validate_password_confirmation
			if password != password_confirmation
				errors.add(:password, 'is not the same as your Confirmation Password')
			end
		end

		def hash_password
			self.password = Hashing.create(self.password)
		end

		def password_is_complex
			c = 0

			# we will increment unless no match
			c += 1 unless nil === (password =~ /\d+/)
			c += 1 unless nil === (password =~ /[a-z]+/)
			c += 1 unless nil === (password =~ /[A-Z]+/)
			c += 1 unless nil === (password =~ /[^a-zA-Z\d]+/)

			if c < 2
				errors.add(:password, 'not complex enough - try adding numbers or special characters')
			end
		end
end
