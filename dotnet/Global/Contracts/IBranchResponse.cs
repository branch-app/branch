namespace Branch.Global.Contracts
{
	public interface IBranchResponse
	{
		/// <summary>
		/// The cache information about this resource.
		/// </summary>
		CacheInfo CacheInfo { get; set; }
	}
}
