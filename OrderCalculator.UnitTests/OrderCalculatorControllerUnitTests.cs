using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderCalculator.App;
using System;
using System.IO;
using System.Text;

namespace OrderCalculator.UnitTests
{
	[TestClass]
	public class OrderCalculatorControllerUnitTests
	{
		[TestMethod]
		public void ReadEmptyArgumentsFromConsole()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				using (var sr = new StringReader(string.Empty))
				{
					Console.SetIn(sr);

					var result = Program.Main(new string[] { });

					var sb = new StringBuilder();

					sb.AppendLine("Please enter a command line.");
					sb.AppendLine("Command format example: CalculateOrder CatalogFilePath Product1 QuantityProduct1 Product2 QuantityProduct2...");

					sb.AppendLine("Please enter a command line.");
					sb.AppendLine("Command format example: CalculateOrder CatalogFilePath Product1 QuantityProduct1 Product2 QuantityProduct2...");

					sb.AppendLine("Please enter a command line.");
					sb.AppendLine("Command format example: CalculateOrder CatalogFilePath Product1 QuantityProduct1 Product2 QuantityProduct2...");

					sb.AppendLine("Maximum number of attempts reached (3). Please try again later.");

					var expected = sb.ToString();
					var actual = sw.ToString();

					Assert.AreEqual(expected, actual);
					Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
				}
			}
		}

		[TestMethod]
		public void MinimumArguments()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder" };
				var result = Program.Main(args);

				var expected = "An error occurred while parsing the arguments which have been provided -> Please provide at least 4 arguments.\r\n";
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void OddArguments()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.csv", "P4", "6", "P10" };
				var result = Program.Main(args);

				var expected = "An error occurred while parsing the arguments which have been provided -> Number of arguments can't be odd.\r\n";
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void InvalidOperation()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "Calculate", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.csv", "P4", "6", "P10", "5" };
				var result = Program.Main(args);

				var expected = "An error occurred while parsing the arguments which have been provided -> Please provide a valid operation.\r\n";
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void FileNotFound()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.file", "P4", "6", "P10", "5" };
				var result = Program.Main(args);

				var expected = "An error occurred while parsing the arguments which have been provided -> Please provide an existent catalog file.\r\n";
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void ExtensionNotAllowed()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.test", "P4", "6", "P10", "5" };
				var result = Program.Main(args);

				var expected = "An error occurred while parsing the arguments which have been provided -> Please provide a file which contains an allowed extension (.txt,.csv).\r\n";
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void NotIntegerValue()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.csv", "P4", "6", "P10", "a" };
				var result = Program.Main(args);

				var expected = "An error occurred while parsing the arguments which have been provided -> The value 'a' shall be an integer.\r\n";
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void ProductNotFound()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.productnotfound.csv", "P4", "6", "P10", "5", "P12", "1" };
				var result = Program.Main(args);

				var sb = new StringBuilder();

				sb.AppendLine("An error occurred while calculating the total price of an order -> Please take a look below for getting further information.");
				sb.AppendLine("An error occurred while validating the data set -> Product P12 doesn't exist.");

				var expected = sb.ToString();
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void DuplicatedProducts()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.duplicated.csv", "P4", "6", "P10", "5", "P12", "1" };
				var result = Program.Main(args);

				var sb = new StringBuilder();

				sb.AppendLine("An error occurred while calculating the total price of an order -> Please take a look below for getting further information.");
				sb.AppendLine("An error occurred while validating the data set -> There are 2 products which have the same id: P12.");

				var expected = sb.ToString();
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void OutOfStock()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.outofstock.csv", "P4", "6", "P10", "5", "P12", "1" };
				var result = Program.Main(args);

				var sb = new StringBuilder();

				sb.AppendLine("An error occurred while calculating the total price of an order -> Please take a look below for getting further information.");
				sb.AppendLine("An error occurred while validating the data set -> Product P12 is out of stock.");

				var expected = sb.ToString();
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void PriceNotFound()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.pricenotfound.csv", "P4", "6", "P10", "5", "P12", "1" };
				var result = Program.Main(args);

				var sb = new StringBuilder();

				sb.AppendLine("An error occurred while calculating the total price of an order -> Please take a look below for getting further information.");
				sb.AppendLine("An error occurred while validating the data set -> Product P12 doesn't have price.");

				var expected = sb.ToString();
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void InvalidDataSet()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				var args = new[] { "CalculateOrder", $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.invalid.csv", "P4", "6", "P10", "5", "P12", "1" };
				var result = Program.Main(args);

				var sb = new StringBuilder();

				sb.AppendLine("An error occurred while calculating the total price of an order -> Please take a look below for getting further information.");
				sb.AppendLine("In memory set hasn't been built properly. Please open the file and check the values out.");

				var expected = sb.ToString();
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Error.GetHashCode(), result);
			}
		}

		[TestMethod]
		public void CalculateOrder()
		{
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);

				// File Path:  $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\sample.catalog.csv" or @"Files\sample.catalog.csv"

				var args = new[] { "CalculateOrder", @"Files\sample.catalog.csv", "P4", "6", "P10", "5", "P12", "1" };
				var result = Program.Main(args);

				var expected = "Total: 4151.25\r\n";
				var actual = sw.ToString();

				Assert.AreEqual(expected, actual);
				Assert.AreEqual(ExitCode.Success.GetHashCode(), result);
			}
		}
	}
}
