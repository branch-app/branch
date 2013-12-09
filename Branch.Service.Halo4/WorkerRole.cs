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
			{TaskEntity.TaskType.Auth, new TimeSpan(0, 45, 0)}
		};

		public override void Run()
		{
			Trace.TraceInformation("Branch.Service.Halo4 entry point called", "Information");
			var storage = new TableStorage();
			var h4WaypointManager = new WaypointManager(storage);

			// Update Stuff
			IEnumerable<TaskEntity> tasks = storage.RetrieveMultipleEntities<TaskEntity>("Halo4ServiceTasks",
				storage.Halo4ServiceTasksCloudTable);

			#region Check to Execute Tasks

			foreach (TaskEntity task in tasks.Where(task => DateTime.UtcNow >= (task.LastRun.AddSeconds(task.Interval))))
			{
				switch (task.Type)
				{
					case TaskEntity.TaskType.Auth:
						bool auth = I343.UpdateAuthentication(storage);
						if (auth)
						{
							task.LastRun = DateTime.UtcNow;
							task.Interval = (int) new TimeSpan(0, 45, 0).TotalSeconds;
						}
						else
						{
							task.LastRun = DateTime.UtcNow;
							task.Interval = (int) new TimeSpan(0, 20, 0).TotalSeconds;
						}
						break;
				}

				storage.UpdateEntity(task, storage.Halo4ServiceTasksCloudTable);
			}

			#endregion

			// Sleep for 5 minutes until we need to update stuff again. yolo
			Thread.Sleep(TimeSpan.FromMinutes(5));
		}

		public override bool OnStart()
		{
			Trace.TraceInformation("Branch.Service.Halo4 service started", "Information");
			ServicePointManager.DefaultConnectionLimit = 1;
			var storage = new TableStorage();

			#region Create Tasks if they don't exist

			foreach (TaskEntity entity in from task in _tasks
				let entity =
					storage.RetrieveSingleEntity<TaskEntity>("Halo4ServiceTasks", TaskEntity.FormatRowKey(task.Key.ToString()),
						storage.Halo4ServiceTasksCloudTable)
				where entity == null
				select new TaskEntity(task.Key)
				{
					LastRun = new DateTime(1994, 8, 18),
					Interval = (int) task.Value.TotalSeconds
				})
			{
				storage.InsertSingleEntity(entity, storage.Halo4ServiceTasksCloudTable);
			}

			#endregion

			return base.OnStart();
		}
	}
}