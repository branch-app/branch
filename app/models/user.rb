class User < ActiveRecord::Base
	attr_accessible :email, :gamertag_id, :gamertag, :name, :password, :password_confirmation, :role_id, :username
	attr_accessor :password_confirmation, :gamertag

	has_many :session

	validates_presence_of :email, :gamertag, :name, :password
	validates_presence_of :password_confirmation, on: :create

	validates_length_of :password, if: ->(a) { a.password.present? }, minimum: 7
	validates_length_of :username, in: 1..30
	validates_length_of :name, in: 3..30

	validates_uniqueness_of :email, :username

	validate :validate_password_confirmation, on: :create
	validate :map_gamertag
	validate :password_is_complex, if: ->(a) { a.password.present? }

	validates_format_of :email, with: /\A[\w\d\-.+]+@[\w\d\-.+]+\.[a-z]{2,25}\Z/i
	validates_format_of :username, with: /\A\w+\Z/
	validates_format_of :name, with: /\A[A-Za-z\- ]+\z/
	validates_format_of :gamertag, with: /\A[a-z]([a-z0-9]{0,15} ?)*\Z/i

	before_create :hash_password

	def self.authenticate(identifier, password)
		user = find_by_username(identifier)
		user = find_by_email(identifier) if user == nil

		return user if user == nil
		return user if Hashing.validate(password, user.password)
	end

	private
		def map_gamertag
			g_tag = Gamertag.find_by_gamertag(gamertag)
			if (g_tag == nil)
				g_tag = Gamertag.new(gamertag: gamertag)
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
