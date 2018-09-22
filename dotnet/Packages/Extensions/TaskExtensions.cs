using System;
using System.Threading.Tasks;

namespace Branch.Packages.Extensions
{
	public static class TaskExt
	{
		/// <summary>
		/// Takes in a function that returns a task. It will then run the task without
		/// awaiting the result.
		/// </summary>
		/// <param name="func">The function that contains the task.</param>
		public static void FireAndForget(Func<Task> func)
		{
			Task.Run(func).ConfigureAwait(false);
		}
	}
}
