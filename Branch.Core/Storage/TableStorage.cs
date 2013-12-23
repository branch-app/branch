using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			// le Branch
			BranchCloudTable = TableClient.GetTableReference("Branch");
			BranchCloudTable.CreateIfNotExists();

			// le Auth
			AuthenticationCloudTable = TableClient.GetTableReference("Authentication");
			AuthenticationCloudTable.CreateIfNotExists();

			// le Halo 4
			Halo4ServiceTasksCloudTable = TableClient.GetTableReference("Halo4ServiceTasks");
			Halo4ServiceTasksCloudTable.CreateIfNotExists();
			Halo4CloudTable = TableClient.GetTableReference("Halo4");
			Halo4CloudTable.CreateIfNotExists();

			#endregion
		}

		#region Entity Operations

		/// <summary>
		/// </summary>
		/// <param name="dataEntity"></param>
		/// <param name="cloudTable"></param>
		public bool InsertSingleEntity(BaseEntity dataEntity, CloudTable cloudTable)
		{
			var insertOperation = TableOperation.Insert(dataEntity);
			var operationResult = cloudTable.Execute(insertOperation);
			return (operationResult.HttpStatusCode == 201);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataEntity"></param>
		/// <param name="cloudTable"></param>
		public void InsertOrReplaceSingleEntity(BaseEntity dataEntity, CloudTable cloudTable)
		{
			try
			{
				var insertOrReplaceOperation = TableOperation.InsertOrReplace(dataEntity);
				cloudTable.Execute(insertOrReplaceOperation);
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="dataEntity"></param>
		/// <param name="cloudTable"></param>
		public void ReplaceSingleEntity(object dataEntity, CloudTable cloudTable)
		{
			var updateOperation = TableOperation.Replace((TableEntity)dataEntity);
			cloudTable.Execute(updateOperation);
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

		#endregion

		// Tables
		public CloudTable BranchCloudTable { get; private set; }
		public CloudTable AuthenticationCloudTable { get; private set; }
		public CloudTable Halo4ServiceTasksCloudTable { get; private set; }
		public CloudTable Halo4CloudTable { get; private set; }
	}
}
