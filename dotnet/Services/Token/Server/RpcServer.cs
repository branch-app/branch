using System.Threading.Tasks;
using Branch.Services.Token.App;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Token
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
