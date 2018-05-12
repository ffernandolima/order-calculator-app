using System;

namespace OrderCalculator.App.Exceptions
{
	internal class OrderCalculatorException : Exception
	{
		private const string ExceptionMessage = "An error occurred while calculating the total price of an order";

		public OrderCalculatorException(string message)
			: base(GetMessage(message))
		{ }

		public OrderCalculatorException(string message, Exception innerException)
			: base(GetMessage(message), innerException)
		{ }

		private static string GetMessage(string message)
		{
			return $"{ExceptionMessage} -> {message ?? "Please take a look below for getting further information."}";
		}
	}
}
