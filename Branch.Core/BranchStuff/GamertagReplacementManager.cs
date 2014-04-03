using Branch.Core.Storage;
using Branch.Models.Services.Branch;
using Microsoft.WindowsAzure.Storage.Table;

namespace Branch.Core.BranchStuff
{
	public static class GamerIdReplacementManager
	{
		/// <summary>
		/// Returns the non-replacement gamer id
		/// </summary>
		public static string GetOriginalGamerId(string gamerId, AzureStorage storage, GamerId gamerIdType = GamerId.X360XblGamertag)
		{
			var replacement =
				storage.Table.QueryAndRetrieveSingleEntity<GamerIdReplacementEntity>(
					TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GamerIdReplacementEntity.PartitionKeyString),
					TableOperators.And,
					TableQuery.GenerateFilterConditionForInt("TypeInt", QueryComparisons.Equal, (int)gamerIdType),
					TableOperators.And,
					TableQuery.GenerateFilterCondition("SafeReplacementId", QueryComparisons.Equal, gamerId.ToLowerInvariant()),
					storage.Table.BranchCloudTable);

			return replacement != null ? replacement.OriginalId : gamerId;
		}

		/// <summary>
		/// Returns the replacement gamer id
		/// </summary>
		public static string GetReplacementGamerId(string gamerId, AzureStorage storage, GamerId gamerIdType = GamerId.X360XblGamertag)
		{
			var replacement =
				storage.Table.QueryAndRetrieveSingleEntity<GamerIdReplacementEntity>(
					TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GamerIdReplacementEntity.PartitionKeyString),
					TableOperators.And,
					TableQuery.GenerateFilterConditionForInt("TypeInt", QueryComparisons.Equal, (int)gamerIdType),
					TableOperators.And,
					TableQuery.GenerateFilterCondition("Id", QueryComparisons.Equal, gamerId.ToLowerInvariant()),
					storage.Table.BranchCloudTable);

			return replacement != null ? replacement.ReplacementId : gamerId;
		}
	}
}
