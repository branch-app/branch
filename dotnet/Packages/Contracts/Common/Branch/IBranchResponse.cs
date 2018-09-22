using System;

namespace Branch.Packages.Contracts.Common.Branch
{
	public interface IBranchResponse
	{
		/// <summary>
		/// The cache information about this resource.
		/// </summary>
		ICacheInfo CacheInfo { get; set; }
	}
}
