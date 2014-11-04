namespace Branch.App.Helpers.Razor
{
	public static class CdnHelpers
	{
		public static string GetAssetLocale(bool preCompiled = true)
		{
#if DEBUG
			return preCompiled ? "~/" : "/";
#else
			return "//az673231.vo.msecnd.net/";
#endif
		}
	}
}