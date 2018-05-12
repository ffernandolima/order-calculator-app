using System;
using System.Collections.Generic;

namespace OrderCalculator.Framework.Extensions
{
	public static class ExceptionExtensions
	{
		public static IEnumerable<Exception> InnerExceptions(this Exception exception)
		{
			var ex = exception;

			while (ex != null)
			{
				yield return ex;

				ex = ex.InnerException;
			}
		}
	}
}
