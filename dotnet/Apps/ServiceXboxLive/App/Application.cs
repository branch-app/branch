using Branch.Clients.Token;

namespace Branch.Apps.ServiceXboxLive.App
{
	public partial class Application
	{
		private readonly TokenClient _tokenClient;

		public Application(TokenClient tokenClient)
		{
			this._tokenClient = tokenClient;
		}
	}
}
