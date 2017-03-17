module ApplicationHelper
	def asset_exists?(path)
		if Rails.configuration.assets.compile
			return Rails.application.precompiled_assets.include?(path)
		end

		return Rails.application.assets_manifest.assets[path].present?()
	end

	def public_asset_exists?(path)
		return FileTest.exist?("#{Rails.public_path}/images/#{path}")
	end

	def is_active_exp_area(exp)
		return 'active' if @area[:exp_slug] == exp
	end

	def generate_percentage(a, b, places = 2)
		return ((a.to_f / b.to_f) * 100).round(places)
	end

	def str_to_date_str(str, format = '%A, %B %e %Y')
		str.to_time.strftime(format)
	end

	def calculate_kd_ratio(kills, deaths, round = 2)
		return kills if deaths == 0
		return (kills.to_f / deaths.to_f).round(round) 
	end
end
