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
end
