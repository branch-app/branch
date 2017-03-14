module Halo4::HomeHelper
	def resolve_asset_url(asset, size)
		asset_url = asset.asset_url.sub!('{size}', size)
		url = Halo4Client.instance.metadata_options['settings'][asset.baseUrl]
		local_asset = Rails.application.assets.find_asset(asset.baseUrl + asset_url)
		if local_asset != nil
			return local_asset
		else
			return url + asset_url
		end
	end
end
