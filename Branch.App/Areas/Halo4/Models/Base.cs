﻿using Branch.Models.Services.Halo4._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class Base
	{
		public Base(ServiceRecord serviceRecord)
		{
			ServiceRecord = serviceRecord;
		}

		public ServiceRecord ServiceRecord { get; set; }
	}
}