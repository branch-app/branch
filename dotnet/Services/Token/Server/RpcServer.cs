using Branch.Services.Token.App;

namespace Branch.Services.Token.Server
{
	public partial class RpcServer : ITokenService
	{
		private Application _app;

		public RpcServer(Application app)
		{
			_app = app;
		}
	}
}
