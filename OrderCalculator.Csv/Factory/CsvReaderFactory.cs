using CsvHelper;
using OrderCalculator.Csv.Config;
using System;
using System.IO;
using System.Text;

namespace OrderCalculator.Csv.Factory
{
	public class CsvReaderFactory
	{
		public static CsvReader GetReader(FileInfo fileInfo, Type type)
		{
			var streamReader = new StreamReader(fileInfo.FullName, Encoding.UTF8);

			var configuration = CsvHelperConfig.GetConfiguration();

			var map = CsvHelperConfig.GetMapping(type);

			if (map != null)
			{
				configuration.RegisterClassMap(map);
			}

			var reader = new CsvReader(streamReader, configuration);

			return reader;
		}
	}
}
