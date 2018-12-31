using System.Threading.Tasks;
using Branch.Apps.ServiceXboxLive.App;
using Branch.Packages.Contracts.ServiceXboxLive;

namespace Branch.Apps.ServiceXboxLive.Server
{
	public partial class RPC : IService
	{
		private Application app { get; }

		public RPC(Application app)
		{
			this.app = app;
		}
	}
}
