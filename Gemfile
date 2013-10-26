source 'https://rubygems.org'

# Frameworks
gem 'rails', '3.2.13'

# Database
gem 'mysql2', '0.3.11'

# Security
gem 'pbkdf2'

# Gems used only for assets and not required
# in production environments by default.
group :assets do
  gem 'sass-rails',   '~> 3.2.3'
  gem 'uglifier', '>= 1.0.3'
end

group :development, :test do
	gem 'thin'
end

group :production do
	gem 'unicorn'
end

# Front End
gem 'sass'

# Other
gem 'redcarpet'
gem 'rufus-scheduler'
gem 'httparty', '0.11.0' # new version does weird shit with https
gem 'modern-user-agent'
gem 'geokit', '1.6.5'
gem 'figaro'
gem 'nokogiri'

# AWS
gem 'aws-sdk'