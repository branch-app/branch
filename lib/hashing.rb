require 'yaml'

module Hashing
	settings = YAML::load_file "#{Rails.root}/config/hashing.yml"
	@hash_length = settings['hash_length']
	@salt_length = settings['salt_length']
	@iterations = settings['iterations']

	def self.validate(plain_text, hash_data)
		begin
			hash_data_seperated = hash_data.split(':')
			hash = Base64.strict_decode64(hash_data_seperated[0])
			salt = Base64.strict_decode64(hash_data_seperated[1])
			hash_length = hash.length

			hash == encrypt(plain_text, salt, hash_data_seperated[2].to_i, hash_length)
		rescue
			false
		end
	end

	def self.create(plain_text)
		salt = make_salt

		"#{Base64.strict_encode64(encrypt(plain_text, salt))}:#{Base64.strict_encode64(salt)}:#{@iterations}"
	end

	private

	def self.encrypt(plain_text, salt, iterations = @iterations, hash_length = @hash_length)
		derived_key = PBKDF2.new do |key|
			key.password = plain_text
			key.salt = salt
			key.iterations = iterations
			key.key_length = hash_length
		end

		derived_key.bin_string
	end

	def self.make_salt
		SecureRandom.random_bytes(@salt_length)
	end
end
