# crpc

Simple plug-and-play CRPC middleware for ASP.NET core. Dependable and Injectable.

## Usage

I'll write more documentation on this later. But this should get you going.

```csharp
public class Startup
{
	public IConfiguration Configuration { get; }

	public Startup(IConfiguration configuration)
	{
		// Currently this configuration handover is required, I'll look for a way to do it behind the scenes later.
		Configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.Configure<Config>(Configuration);

		// Register the RPC server layer to it's service interface
		services.AddSingleton<IService, RPC>();

		services.AddCrpc(opts =>
		{
			opts.InternalKeys = Configuration.GetValue<string[]>("InternalKeys");
		});
	}

	public void Configure(IApplicationBuilder app)
	{
		app.UseCrpc("/1", opts => {
			// Register the server type - needed for reflection later
			opts.RegisterServer<RPC>();

			// Register a method. endpoint, method name, date (yyyy-mm-dd or "preview")
			opts.RegisterMethod<object, string>("get_testing_thingy", "GetTestingThingy", "preview");
		});
	}

	public static async Task Main(string[] args) =>
		await CrpcHost.CreateCrpcHost<Startup>()
			.Build()
			.RunAsync();
}

public class RPC
{
	public async Task<string> GetTestingThingy(object req)
	{
		await Task.Delay(1);

		return "ya boii";
	}

	public static readonly string GetTestingThingySchema = @"
		{
			""type"": ""object"",
			""additionalProperties"": false,

			""required"": [
				""testing""
			],

			""properties"": {
				""testing"": {
					""type"": ""bool""
				}
			}
		}
	";
}
```

## License

This code is based on (and compatible with) the CRPC (Cuvva-RPC) framework that was developed in-house at [Cuvva](https://github.com/cuvva).
