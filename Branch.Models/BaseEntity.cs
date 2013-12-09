using Microsoft.WindowsAzure.Storage.Table;

namespace Branch.Models
{
	public abstract class BaseEntity : TableEntity
	{
		public virtual void SetKeys(string partitionKey, string rowKey)
		{
			PartitionKey = partitionKey;
			RowKey = rowKey;
		}
	}
}
