using Branch.Services.Identity.App;

namespace Branch.Services.Identity.Server
{
	public partial class RpcServer : IIdentityService
	{
		private Application _app;

		public RpcServer(Application app)
		{
			_app = app;
		}
	}
}
