module S3Storage
	@s3 = AWS::S3.new

	def self.push(game, type, name, data)
		init

		bucket_name = "branchapp_storage/#{game}/#{type}"
		bucket = @s3.buckets[bucket_name]
		@s3.buckets.create(bucket_name) unless bucket.exists?
		object = bucket.objects[name]
		object.write data
	end

	def self.pull(game, type, name)
		init

		begin
			bucket_name = "branchapp_storage/#{game}/#{type}"
			bucket = @s3.buckets[bucket_name]
			@s3.buckets.create(bucket_name) unless bucket.exists?
			object = bucket.objects[name]
			data = object.read

			data
		rescue
			nil
		end
	end

	@is_initalized = false
	def self.init
		return if @is_initalized

		AWS.config(
			:access_key_id => ENV['S3_ACCESS_KEY_ID'],
			:secret_access_key => ENV['S3_SECRET_KEY']
		)
		@s3 = AWS::S3.new

		@is_initalized = true
	end
end
