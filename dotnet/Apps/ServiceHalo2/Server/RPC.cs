using System.Threading.Tasks;
using Branch.Apps.ServiceHalo2.App;
using Branch.Packages.Contracts.ServiceHalo2;

namespace Branch.Apps.ServiceHalo2.Server
{
	public partial class RPC : IService
	{
		private Application _app { get; }

		public RPC(Application app)
		{
			this._app = app;
		}
	}
}
