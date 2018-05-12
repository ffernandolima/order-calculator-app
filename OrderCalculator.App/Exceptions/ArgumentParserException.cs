using System;

namespace OrderCalculator.App.Exceptions
{
	internal class ArgumentParserException : Exception
	{
		private const string ExceptionMessage = "An error occurred while parsing the arguments which have been provided";

		public ArgumentParserException(string message)
			: base(GetMessage(message))
		{ }

		public ArgumentParserException(string message, Exception innerException)
			: base(GetMessage(message), innerException)
		{ }

		private static string GetMessage(string message)
		{
			return $"{ExceptionMessage} -> {message ?? "Please take a look below for getting further information."}";
		}
	}
}
