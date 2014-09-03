using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Branch.Extenders;
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
			Halo4CloudTable = TableClient.GetTableReference("Halo4");
			Halo4CloudTable.CreateIfNotExists();

			// le Halo: Reach
			HReachCloudTable = TableClient.GetTableReference("HReach");
			HReachCloudTable.CreateIfNotExists();

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
			try
			{
				var operationResult = cloudTable.Execute(insertOperation);
				return (operationResult.HttpStatusCode == 201);
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// Inserts or Replaces a collection of entities into the specified Cloud Table.
		/// </summary>
		/// <param name="dataEntities">The collection of entities to insert or replace.</param>
		/// <param name="cloudTable">The cloud table to insert or replace into.</param>
		public void InsertOrReplaceMultipleEntities(IEnumerable<BaseEntity> dataEntities, CloudTable cloudTable)
		{
			foreach (var chunk in dataEntities.Chunk(100))
			{
				var operation = new TableBatchOperation();
				foreach (var dataEntity in chunk)
					operation.InsertOrReplace(dataEntity);

				try
				{
					cloudTable.ExecuteBatch(operation);
				}
				catch (Exception ex)
				{
					Trace.TraceError(ex.ToString());
				}
			}
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


			if (operationResult.Result != null && operationResult.Result.GetType() == typeof(TEntityType))
				return (TEntityType)operationResult.Result;


			return null;
		}


		/// <summary>
		/// </summary>
		/// <typeparam name="TEntityType"></typeparam>
		/// <param name="query"></param>
		/// <param name="cloudTable"></param>
		/// <returns></returns>
		public TEntityType QueryAndRetrieveSingleEntity<TEntityType>(string query, CloudTable cloudTable)
			where TEntityType : BaseEntity, new()
		{
			var rangeQuery = new TableQuery<TEntityType>().Where(query);
			var result = cloudTable.ExecuteQuery(rangeQuery).ToList();


			return result.Any() ? result.First() : null;
		}


		/// <summary>
		/// </summary>
		/// <typeparam name="TEntityType"></typeparam>
		/// <param name="queryA"></param>
		/// <param name="op"></param>
		/// <param name="queryB"></param>
		/// <param name="cloudTable"></param>
		/// <returns></returns>
		public TEntityType QueryAndRetrieveSingleEntity<TEntityType>(string queryA, string op, string queryB, CloudTable cloudTable)
			where TEntityType : BaseEntity, new()
		{
			return QueryAndRetrieveSingleEntity<TEntityType>(
				TableQuery.CombineFilters(queryA, op, queryB), cloudTable);
		}
		

		/// <summary>
		/// </summary>
		/// <typeparam name="TEntityType"></typeparam>
		/// <param name="queryA"></param>
		/// <param name="op1"></param>
		/// <param name="queryB"></param>
		/// <param name="op2"></param>
		/// <param name="queryC"></param>
		/// <param name="cloudTable"></param>
		/// <returns></returns>
		public TEntityType QueryAndRetrieveSingleEntity<TEntityType>(string queryA, string op1, string queryB, string op2, string queryC, CloudTable cloudTable)
			where TEntityType : BaseEntity, new()
		{
			return QueryAndRetrieveSingleEntity<TEntityType>(
				TableQuery.CombineFilters(TableQuery.CombineFilters(queryA, op1, queryB), op2, queryC), cloudTable);
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
		/// 
		/// </summary>
		/// <typeparam name="TEntityType"></typeparam>
		/// <param name="query"></param>
		/// <param name="cloudTable"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public IEnumerable<TEntityType> QueryAndRetrieveMultipleEntities<TEntityType>(string query, CloudTable cloudTable, int count = -1)
			where TEntityType : BaseEntity, new()
		{
			var rangeQuery = new TableQuery<TEntityType>().Where(query);
			var result = cloudTable.ExecuteQuery(rangeQuery);
			var output = new List<TEntityType>();


			var index = 0;
			count = (count < 0) ? 0 : count;
			foreach (var item in result)
			{
				output.Add(item);
				index++;
				if (index == (count)) break;
			}


			return output;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TEntityType"></typeparam>
		/// <param name="queryA"></param>
		/// <param name="op"></param>
		/// <param name="queryB"></param>
		/// <param name="cloudTable"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public IEnumerable<TEntityType> QueryAndRetrieveMultipleEntities<TEntityType>(string queryA, string op, string queryB, CloudTable cloudTable, int count = -1)
			where TEntityType : BaseEntity, new()
		{
			return QueryAndRetrieveMultipleEntities<TEntityType>(
				TableQuery.CombineFilters(queryA, op, queryB), cloudTable, count);
		}


		#endregion

		// Tables
		public CloudTable BranchCloudTable { get; private set; }
		public CloudTable AuthenticationCloudTable { get; private set; }
		public CloudTable Halo4CloudTable { get; private set; }
		public CloudTable HReachCloudTable { get; private set; }
	}
}
