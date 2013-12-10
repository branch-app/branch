using System.Collections.Generic;
using Branch.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Branch.Core.Storage
{
	/// <summary>
	/// </summary>
	public class TableStorage
	{
		public CloudTableClient TableClient { get; private set; }
		public CloudStorageAccount StorageAccount { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public TableStorage(CloudStorageAccount storage)
		{
			StorageAccount = storage;

			// Attempt to set up shit
			TableClient = StorageAccount.CreateCloudTableClient();

			#region Initialize Tables

			AuthenticationCloudTable = TableClient.GetTableReference("Authentication");
			AuthenticationCloudTable.CreateIfNotExists();

			Halo4ServiceTasksCloudTable = TableClient.GetTableReference("Halo4ServiceTasks");
			Halo4ServiceTasksCloudTable.CreateIfNotExists();

			#endregion
		}

		#region Entity Operations

		/// <summary>
		/// </summary>
		/// <param name="dataEntity"></param>
		/// <param name="cloudTable"></param>
		public bool InsertSingleEntity(object dataEntity, CloudTable cloudTable)
		{
			var insertOperation = TableOperation.Insert((TableEntity)dataEntity);
			var operationResult = cloudTable.Execute(insertOperation);
			return (operationResult.HttpStatusCode == 201);
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="TEntityType"></typeparam>
		/// <param name="partitionKey"></param>
		/// <param name="rowKey"></param>
		/// <param name="cloudTable"></param>
		/// <returns></returns>
		public TEntityType RetrieveSingleEntity<TEntityType>(string partitionKey, string rowKey, CloudTable cloudTable)
			where TEntityType : BaseEntity
		{
			var retrieveOperation = TableOperation.Retrieve<TEntityType>(partitionKey, rowKey);
			var operationResult = cloudTable.Execute(retrieveOperation);

			if (operationResult.Result != null && operationResult.Result.GetType() == typeof (TEntityType))
				return (TEntityType) operationResult.Result;

			return null;
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="TEntityType"></typeparam>
		/// <param name="partitionKey"></param>
		/// <param name="cloudTable"></param>
		/// <returns></returns>
		public IEnumerable<TEntityType> RetrieveMultipleEntities<TEntityType>(string partitionKey, CloudTable cloudTable)
			where TEntityType : BaseEntity, new()
		{
			var rangeQuery =
				new TableQuery<TEntityType>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
					partitionKey));

			return cloudTable.ExecuteQuery(rangeQuery);
		}

		/// <summary>
		/// </summary>
		/// <param name="dataEntity"></param>
		/// <param name="cloudTable"></param>
		public void UpdateEntity(object dataEntity, CloudTable cloudTable)
		{
			var updateOperation = TableOperation.Replace((TableEntity)dataEntity);
			cloudTable.Execute(updateOperation);
		}

		#endregion

		// Tables
		public CloudTable AuthenticationCloudTable { get; private set; }
		public CloudTable Halo4ServiceTasksCloudTable { get; private set; }
	}
}