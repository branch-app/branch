using System;

namespace Branch.App.Models
{
	public class ErrorViewModel
	{
		public ErrorViewModel(Exception exception, Guid loggedExceptionGuid,
			int httpStatusCode)
		{
			Exception = exception;
			LoggedExceptionGuid = loggedExceptionGuid;
			HttpStatusCode = httpStatusCode;
		}

		public Exception Exception { get; set; }

		public Guid LoggedExceptionGuid { get; set; }

		public int HttpStatusCode { get; set; }
	}
}