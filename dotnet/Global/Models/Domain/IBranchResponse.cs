namespace Branch.Global.Models.Domain
{
	public interface IBranchResponse
	{
		/// <summary>
		/// The cache information about this resource.
		/// </summary>
		CacheInfo CacheInfo { get; set; }
	}
}
