if (defined?(Rails::Server) && Rails.env.development?)
	I343Auth.update_authentication()
	H4Api.init()
end
