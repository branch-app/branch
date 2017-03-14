module Halo4::HomeHelper
	def resolve_asset_url(asset, size)
		asset_url = asset['assetUrl'].sub!('{size}', size.to_s)
		base_url = asset['baseUrl']

		url = Halo4Client.instance.metadata_options['settings'][base_url]
		local_asset = Rails.application.assets.find_asset(base_url + asset_url)
		if local_asset != nil
			return local_asset
		else
			return url + asset_url
		end
	end

	def resolve_spartan_image(gamertag, pose = 'fullbody', size = 'large')
		return "https://spartans.svc.halowaypoint.com/players/#{gamertag}/h4/spartans/#{pose}?target=#{size}"
	end
end
