using System;

namespace Branch.Packages.Exceptions
{
	public class BranchException : Exception
	{
		public BranchException(string message)
			: base(message) { }
	}
}
