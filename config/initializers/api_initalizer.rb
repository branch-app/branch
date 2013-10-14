if (defined?(Rails::Server) || defined?(Rails::Console))
	I343Auth.update_authentication()
	H4Api.init()
end
