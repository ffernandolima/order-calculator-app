using System;

namespace OrderCalculator.Data.Exceptions
{
	public class DataValidatorException : Exception
	{
		private const string ExceptionMessage = "An error occurred while validating the data set";

		public DataValidatorException(string message)
			: base(GetMessage(message))
		{ }

		public DataValidatorException(string message, Exception innerException)
			: base(GetMessage(message), innerException)
		{ }

		private static string GetMessage(string message)
		{
			return $"{ExceptionMessage} -> {message ?? "Please take a look below for getting further information."}";
		}
	}
}
