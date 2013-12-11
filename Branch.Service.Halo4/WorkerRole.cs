using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Branch.Core.Api.Authentication;
using Branch.Core.Api.Halo4;
using Branch.Core.Storage;
using Branch.Models.Services.Halo4;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Branch.Service.Halo4
{
	public class WorkerRole : RoleEntryPoint
	{
		private readonly Dictionary<TaskEntity.TaskType, TimeSpan> _tasks = new Dictionary<TaskEntity.TaskType, TimeSpan>
		{
			{ TaskEntity.TaskType.Playlist, new TimeSpan(0, 15, 0) },
			{ TaskEntity.TaskType.Auth, new TimeSpan(0, 45, 0) },
			{ TaskEntity.TaskType.Metadata, new TimeSpan(1, 0, 0, 0) },
		};

		private AzureStorage _storage;
		private WaypointManager _h4WaypointManager;

		public override void Run()
		{
			Trace.TraceInformation("Branch.Service.Halo4 entry point called");

			// Update Stuff
			var tasks = _storage.Table.RetrieveMultipleEntities<TaskEntity>("Halo4ServiceTasks",
				_storage.Table.Halo4ServiceTasksCloudTable);

#if DEBUG
			//var doofette = _h4WaypointManager.GetServiceRecord("Doofette");
			//var peaches = _h4WaypointManager.GetServiceRecord("iBotPeaches v5");
			//var test = _h4WaypointManager.GetServiceRecord("AuntieDot Test");
			//var unknown = _h4WaypointManager.GetServiceRecord("65utrfgkt7fj");
			//var erroneous = _h4WaypointManager.GetServiceRecord(")(^&^");
#endif

			#region Check to Execute Tasks

			foreach (var task in tasks.Where(task => DateTime.UtcNow >= (task.LastRun.AddSeconds(task.Interval))))
			{
				switch (task.Type)
				{
					case TaskEntity.TaskType.Playlist:
						_h4WaypointManager.UpdatePlaylists();
						break;

					case TaskEntity.TaskType.Auth:
						var auth = I343.UpdateAuthentication(_storage);
						if (!auth)
							task.Interval = (int) new TimeSpan(0, 15, 0).TotalSeconds;
						break;

					case TaskEntity.TaskType.Metadata:
						_h4WaypointManager.UpdateMetadata();
						break;
				}

				task.Interval = (int)_tasks.First(t => t.Key == task.Type).Value.TotalSeconds;
				task.LastRun = DateTime.UtcNow;
				_storage.Table.ReplaceSingleEntity(task, _storage.Table.Halo4ServiceTasksCloudTable);
			}

			#endregion

			// Sleep for 5 minutes until we need to update stuff again. yolo
			Thread.Sleep(TimeSpan.FromMinutes(10));
		}

		public override bool OnStart()
		{
			Trace.TraceInformation("Branch.Service.Halo4 service started");
			ServicePointManager.DefaultConnectionLimit = 1;
			_storage = new AzureStorage();
			_h4WaypointManager = new WaypointManager(_storage);

			#region Create Tasks if they don't exist

			foreach (var entity in from task in _tasks
				let entity =
					_storage.Table.RetrieveSingleEntity<TaskEntity>("Halo4ServiceTasks", TaskEntity.FormatRowKey(task.Key.ToString()),
						_storage.Table.Halo4ServiceTasksCloudTable)
				where entity == null
				select new TaskEntity(task.Key)
				{
					LastRun = new DateTime(1994, 8, 18),
					Interval = (int) task.Value.TotalSeconds
				})
			{
				_storage.Table.InsertSingleEntity(entity, _storage.Table.Halo4ServiceTasksCloudTable);
			}

			#endregion

			return base.OnStart();
		}
	}
}