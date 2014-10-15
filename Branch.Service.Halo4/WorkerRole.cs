using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Branch.Core.Api.Authentication;
using Branch.Core.Game.Halo4.Api;
using Branch.Core.Storage;
using Branch.Models.Services.Halo4;
using Microsoft.WindowsAzure.ServiceRuntime;

// ReSharper disable ConvertToConstant.Local
// ReSharper disable ConditionIsAlwaysTrueOrFalse
namespace Branch.Service.Halo4
{
	public class WorkerRole : RoleEntryPoint
	{
		private readonly Dictionary<TaskEntity.TaskType, TimeSpan> _tasks = new Dictionary<TaskEntity.TaskType, TimeSpan>
		{
			{ TaskEntity.TaskType.Playlist, new TimeSpan(0, 15, 0) },
			{ TaskEntity.TaskType.Auth, new TimeSpan(0, 45, 0) },
			{ TaskEntity.TaskType.Metadata, new TimeSpan(0, 30, 0) },
			{ TaskEntity.TaskType.Challenge, new TimeSpan(0, 1, 0, 0) },
		};

		private AzureStorage _storage;
		private Manager _h4WaypointManager;

		public override void Run()
		{
			Trace.TraceInformation("Branch.Service.Halo4 entry point called");

#if RELEASE
			try
			{
#endif
				// Update Stuff
				var tasks = _storage.Table.RetrieveMultipleEntities<TaskEntity>(TaskEntity.PartitionKeyString,
					_storage.Table.Halo4CloudTable);

				#region Check to Execute Tasks

				foreach (var task in tasks.Where(task => DateTime.UtcNow >= (task.LastRun.AddSeconds(task.Interval))))
				{
					var updateLastRun = true;

					switch (task.Type)
					{
						case TaskEntity.TaskType.Playlist:
							_h4WaypointManager.UpdatePlaylists();
							break;

						case TaskEntity.TaskType.Auth:
							I343.UpdateAuthentication(_storage);
							break;

						case TaskEntity.TaskType.Metadata:
							_h4WaypointManager.UpdateMetadata();
							break;

						case TaskEntity.TaskType.Challenge:
							_h4WaypointManager.UpdateChallenges();
							break;
					}

					task.Interval = (int) _tasks.First(t => t.Key == task.Type).Value.TotalSeconds;
					if (updateLastRun) task.LastRun = DateTime.UtcNow;
					_storage.Table.ReplaceSingleEntity(task, _storage.Table.Halo4CloudTable);
				}

				#endregion
#if RELEASE
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}
#endif

			// Sleep for 10 minutes until we need to update stuff again. yolo
			Thread.Sleep(TimeSpan.FromMinutes(10));
		}

		public override bool OnStart()
		{
			Trace.TraceInformation("Branch.Service.Halo4 service started");
			ServicePointManager.DefaultConnectionLimit = 1;
			_storage = new AzureStorage();
			_h4WaypointManager = new Manager(_storage, true);

			#region Create Tasks if they don't exist

			foreach (var entity in from task in _tasks
				let entity =
					_storage.Table.RetrieveSingleEntity<TaskEntity>(TaskEntity.PartitionKeyString, TaskEntity.FormatRowKey(task.Key.ToString()),
						_storage.Table.Halo4CloudTable)
				where entity == null
				select new TaskEntity(task.Key)
				{
					LastRun = new DateTime(1994, 8, 18),
					Interval = (int) task.Value.TotalSeconds
				})
			{
				_storage.Table.InsertOrReplaceSingleEntity(entity, _storage.Table.Halo4CloudTable);
			}

			#endregion

			return base.OnStart();
		}
	}
}