using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Branch.Core.Api.Authentication;
using Branch.Core.Storage;
using Branch.Models.Services.Halo4;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Branch.Service.Halo4
{
	public class WorkerRole : RoleEntryPoint
	{
		public override void Run()
		{
			Trace.TraceInformation("Branch.Service.Halo4 entry point called", "Information");
			var storage = new TableStorage();

			// Update Stuff
			var tasks = storage.RetrieveMultipleEntity<TaskEntity>("Halo4ServiceTasks", storage.Halo4ServiceTasksCloudTable);
			foreach (var task in tasks.Where(task => DateTime.UtcNow >= (task.LastRun.AddSeconds(task.Interval))))
			{
				switch (task.Type)
				{
					case TaskEntity.TaskType.Auth:
						var auth = I343.UpdateAuthentication(storage);
						if (auth)
						{
							task.LastRun = DateTime.UtcNow;
							task.Interval = (int)new TimeSpan(0, 45, 0).TotalSeconds;
						}
						else
						{
							task.LastRun = DateTime.UtcNow;
							task.Interval = (int)new TimeSpan(0, 20, 0).TotalSeconds;
						}
						break;
				}

				storage.UpdateEntity(task, storage.Halo4ServiceTasksCloudTable);
			}
		}

		public override bool OnStart()
		{
			Trace.TraceInformation("Branch.Service.Halo4 service started", "Information");
			ServicePointManager.DefaultConnectionLimit = 12;
			var storage = new TableStorage();
			var halo4Api = new Core.Api.Halo4.Halo4(storage);
			Core.Settings.LoadSettings();
			var tasks = new Dictionary<TaskEntity.TaskType, TimeSpan>
			{
				{ TaskEntity.TaskType.Auth, new TimeSpan(0, 45, 0) }
			};

			#region Setup

			foreach (var task in tasks)
			{
				var entity = storage.RetrieveSingleEntity<TaskEntity>("Halo4ServiceTasks", TaskEntity.FormatRowKey(task.Key.ToString()), storage.Halo4ServiceTasksCloudTable);
				if (entity != null) continue;

				entity = new TaskEntity(task.Key)
				{
					LastRun = new DateTime(1994, 8, 18),
					Interval = (int)task.Value.TotalSeconds
				};
				storage.InsertSingleEntity(entity, storage.Halo4ServiceTasksCloudTable);
			}

			#endregion

			#region Run Startup Code

			I343.UpdateAuthentication(storage);

			#endregion

			return base.OnStart();
		}
	}
}
