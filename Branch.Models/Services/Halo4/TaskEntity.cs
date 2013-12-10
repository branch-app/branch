using System;
using System.Collections.Generic;
using Branch.Helpers.Extenders;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Branch.Models.Services.Halo4
{
	public class TaskEntity : BaseEntity
	{
		/// <summary>
		/// </summary>
		public enum TaskType
		{
			Auth = 0x00,
			Metadata = 0x01
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
		public int FakeType { get; set; }
		public TaskType Type
		{
			get { return (TaskType)FakeType; }
			set { FakeType = (int)value; }
		}

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

		#region Overrides

		public override sealed void SetKeys(string partitionKey, string rowKey)
		{
			if (partitionKey == null)
				partitionKey = "Halo4ServiceTasks";

			base.SetKeys(partitionKey, FormatRowKey(rowKey));
		}

		#endregion
	}
}