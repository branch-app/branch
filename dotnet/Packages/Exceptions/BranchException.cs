using System;
using System.Collections;
using System.Collections.Generic;

namespace Branch.Packages.Exceptions
{
	public class BranchException : AggregateException
	{
		public BranchException() { }

		public BranchException(string message) : base(message) { }

		public BranchException(string message, Dictionary<string, object> data)
			: base(message)
		{
			if (data == null)
				return;

			foreach (var pair in data)
				Data.Add(pair.Key, pair.Value);
		}

		public BranchException(string message, Dictionary<string, object> data, Exception ex)
			: base(message, ex)
		{
			if (data == null)
				return;

			foreach (var pair in data)
				Data.Add(pair.Key, pair.Value);
		}

		public BranchException(string message, Dictionary<string, object> data, IEnumerable<Exception> exs)
			: base(message, exs)
		{
			if (data == null)
				return;

			foreach (var pair in data)
				Data.Add(pair.Key, pair.Value);
		}
	}
}
