using Branch.Apps.ServiceHalo4.App;
using Branch.Packages.Contracts.ServiceHalo4;

namespace Branch.Apps.ServiceHalo4.Server
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
