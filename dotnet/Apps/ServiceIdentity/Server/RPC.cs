using System.Threading.Tasks;
using Branch.Apps.ServiceIdentity.App;
using Branch.Packages.Contracts.ServiceIdentity;

namespace Branch.Apps.ServiceIdentity.Server
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
