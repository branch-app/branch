source 'https://rubygems.org'

# Frameworks
gem 'rails', '3.2.13'

# Database
gem 'mysql2', '0.3.11'

# Security
gem 'pbkdf2'

# Amazon
gem "aws-ses", "~> 0.5.0", :require => 'aws/ses'

# Gems used only for assets and not required
# in production environments by default.
group :assets do
  gem 'sass-rails',   '~> 3.2.3'
  gem 'coffee-rails', '~> 3.2.1'
  gem 'uglifier', '>= 1.0.3'
end

group :development, :test do
	gem 'thin'
end

group :production do
	gem 'unicorn'
end

# Front End
gem 'jquery-rails'
gem 'sass'

# Other
gem 'redcarpet'
gem 'rufus-scheduler'
gem 'httparty'
gem 'user-agent'
gem 'geokit', '1.6.5'
gem 'figaro'
