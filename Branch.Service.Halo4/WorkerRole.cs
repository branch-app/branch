using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Branch.Core.Api.Authentication;
using Branch.Core.Api.Halo4;
using Branch.Core.Storage;
using Branch.Models.Services.Branch;
using Branch.Models.Services.Halo4;
using Branch.Models.Services.Halo4.Branch;
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
			{ TaskEntity.TaskType.StatUpdate, new TimeSpan(1, 0, 0, 0) },
			{ TaskEntity.TaskType.Challenge, new TimeSpan(0, 1, 0, 0) },
		};

		private AzureStorage _storage;
		private WaypointManager _h4WaypointManager;

		public override void Run()
		{
			Trace.TraceInformation("Branch.Service.Halo4 entry point called");

#if !DEBUG
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
							var auth = I343.UpdateAuthentication(_storage);
							if (!auth)
								task.Interval = (int) new TimeSpan(0, 15, 0).TotalSeconds;
							break;

						case TaskEntity.TaskType.Metadata:
							_h4WaypointManager.UpdateMetadata();
							break;

						case TaskEntity.TaskType.Challenge:
							_h4WaypointManager.UpdateChallenges();
							break;

						case TaskEntity.TaskType.StatUpdate:
							#region Shink me pls

							var players = _storage.Table.RetrieveMultipleEntities<ServiceRecordEntity>("ServiceRecord",
								_storage.Table.Halo4CloudTable).ToArray();

							// Update Service Records
							var playerServiceRecords = players.Select(player => _h4WaypointManager.GetServiceRecord(player.Gamertag)).ToList();

							var allTimeStats = _storage.Table.RetrieveSingleEntity<Halo4StatsEntity>(Halo4StatsEntity.PartitionKeyString,
								string.Format(Halo4StatsEntity.RowKeyString, Halo4StatType.AllTime), _storage.Table.BranchCloudTable) ??
												new Halo4StatsEntity(Halo4StatType.AllTime);

							var weeklyStats = _storage.Table.RetrieveSingleEntity<Halo4StatsEntity>(Halo4StatsEntity.PartitionKeyString,
								string.Format(Halo4StatsEntity.RowKeyString, Halo4StatType.Weekly), _storage.Table.BranchCloudTable) ??
												new Halo4StatsEntity(Halo4StatType.Weekly);

							var kills = 0;
							var deaths = 0;
							var medals = 0;
							var games = 0;
							var duration = new TimeSpan(0);

							foreach (
								var warGamesStats in
									playerServiceRecords.Select(
										player => player.GameModes.First(m => m.Id == Models.Services.Halo4._343.DataModels.Enums.GameMode.WarGames)))
							{
								kills += warGamesStats.TotalKills;
								deaths += warGamesStats.TotalDeaths;
								medals += warGamesStats.TotalMedals ?? 0;
								games += warGamesStats.TotalGamesStarted;
								duration += TimeSpan.Parse(warGamesStats.TotalDuration ?? TimeSpan.FromTicks(0).ToString());
							}
							
							#region Weekly

							weeklyStats.Players = players.Count();
							weeklyStats.WarGamesKills = kills - allTimeStats.WarGamesKills;
							weeklyStats.WarGamesDeaths = deaths - allTimeStats.WarGamesKills;
							weeklyStats.WarGamesMedals = medals - allTimeStats.WarGamesKills;
							weeklyStats.WarGamesGames = games - allTimeStats.WarGamesKills;
							weeklyStats.WarGamesDuration =
								(duration - TimeSpan.Parse(allTimeStats.WarGamesDuration ?? 
									TimeSpan.FromTicks(0).ToString())).ToString();

							#endregion
							#region All Time

							allTimeStats.Players = players.Count();
							allTimeStats.WarGamesKills += kills;
							allTimeStats.WarGamesDeaths += deaths;
							allTimeStats.WarGamesMedals += medals;
							allTimeStats.WarGamesGames += games;
							allTimeStats.WarGamesDuration = duration.ToString();

							#endregion

							_storage.Table.InsertOrReplaceSingleEntity(weeklyStats, _storage.Table.BranchCloudTable);
							_storage.Table.InsertOrReplaceSingleEntity(allTimeStats, _storage.Table.BranchCloudTable);
							
							#endregion
							break;
					}

					task.Interval = (int) _tasks.First(t => t.Key == task.Type).Value.TotalSeconds;
					if (updateLastRun) task.LastRun = DateTime.UtcNow;
					_storage.Table.ReplaceSingleEntity(task, _storage.Table.Halo4CloudTable);
				}

				#endregion
#if !DEBUG
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
			_h4WaypointManager = new WaypointManager(_storage, true);

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