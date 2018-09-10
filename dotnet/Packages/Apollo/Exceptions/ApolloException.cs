using System;
using System.Collections;
using System.Collections.Generic;

namespace Apollo.Exceptions
{
	public class ApolloException : Exception
	{
		public ApolloException() { }

		public ApolloException(string message) : base(message) { }

		public ApolloException(string message, Exception inner) : base(message, inner) { }

		public ApolloException(string message, Dictionary<string, object> data, Exception inner)
			: base(message, inner)
		{
			foreach (var pair in data)
				Data.Add(pair.Key, pair.Value);
		}

		public ApolloException(string message, Dictionary<string, object> data)
			: base(message)
		{
			foreach (var pair in data)
				Data.Add(pair.Key, pair.Value);
		}
	}
}
