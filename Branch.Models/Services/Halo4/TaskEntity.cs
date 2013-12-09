using System;
using Branch.Helpers.Extenders;

namespace Branch.Models.Services.Halo4
{
	public class TaskEntity : BaseEntity
	{
		/// <summary>
		/// </summary>
		public enum TaskType
		{
			Auth = 0x00
		}

		public const string RowKeyString = "H4Task{0}";

		public TaskEntity()
		{
		}

		public TaskEntity(TaskType type)
		{
			Type = type;
			SetKeys(null, type.ToString());
		}

		/// <summary>
		/// </summary>
		public TaskType Type { get; set; }

		/// <summary>
		/// </summary>
		public int Interval { get; set; }

		/// <summary>
		/// </summary>
		public DateTime LastRun { get; set; }

		public static string FormatRowKey(string ending)
		{
			return String.Format(RowKeyString, ending.ToTitleCase());
		}

		public override sealed void SetKeys(string partitionKey, string rowKey)
		{
			if (partitionKey == null)
				partitionKey = "Halo4ServiceTasks";

			base.SetKeys(partitionKey, FormatRowKey(rowKey));
		}
	}
}