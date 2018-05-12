using OrderCalculator.App.Exceptions;
using OrderCalculator.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrderCalculator.App.Parser
{
	internal static class ArgumentParser
	{
		public static Argument Parse(string[] args)
		{
			ValidateArgumentsCount(args);

			var operation	= ParseOperation(args);
			var catalogFile = ParseCatalogFile(args);
			var items		= ParseItems(args);

			var argument = new Argument(operation, catalogFile, items);

			return argument;
		}

		private static void ValidateArgumentsCount(string[] args)
		{
			const int MinimumArguments = 4;

			const int EvenNumber = 2;
			const int RemainderOfDivision = 0;

			if (args == null || args.Length < MinimumArguments)
			{
				throw new ArgumentParserException("Please provide at least 4 arguments.");
			}

			if (args.Length % EvenNumber != RemainderOfDivision)
			{
				throw new ArgumentParserException("Number of arguments can't be odd.");
			}
		}

		private static Operation ParseOperation(string[] args)
		{
			const Operation UnknownOperation = Operation.Unknown;

			var operation = EnumExtensions.GetValueFromDescription<Operation>(args[0]);

			if (operation == UnknownOperation)
			{
				throw new ArgumentParserException("Please provide a valid operation.");
			}

			return operation;
		}

		private static FileInfo ParseCatalogFile(string[] args)
		{
			var allowedExtensions = new[] { ".txt", ".csv" };

			var catalogFile = new FileInfo(args[1]);

			if (!catalogFile.Exists)
			{
				throw new ArgumentParserException("Please provide an existent catalog file.");
			}

			if (!allowedExtensions.Contains(catalogFile.Extension, StringComparer.OrdinalIgnoreCase))
			{
				throw new ArgumentParserException($"Please provide a file which contains an allowed extension ({string.Join(",", allowedExtensions)}).");
			}

			return catalogFile;
		}

		private static IDictionary<string, int?> ParseItems(string[] args)
		{
			const int SkipCount = 2;

			var items = new Dictionary<string, int?>();

			var itemsArray = args.Skip(SkipCount).Take(args.Length - SkipCount).ToArray();

			for (var i = 0; i < itemsArray.Length; i += 2)
			{
				var key = itemsArray[i];

				var value = itemsArray[i + 1];

				if (string.IsNullOrWhiteSpace(key))
				{
					throw new ArgumentParserException($"Key can't be null or white-space.");
				}

				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentParserException($"Value can't be null or white-space.");
				}

				if (!int.TryParse(value, out int result))
				{
					throw new ArgumentParserException($"The value '{value}' shall be an integer.");
				}

				items.Add(key, result);
			}

			return items;
		}
	}
}
