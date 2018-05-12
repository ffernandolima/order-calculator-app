using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;

namespace OrderCalculator.Csv.TypeConverters
{
	internal class NullIntConverter : Int32Converter
	{
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
		{
			if (memberMapData == null || memberMapData.TypeConverterOptions == null)
			{
				throw new ArgumentException("Some arguments are missing.");
			}

			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}

			if (int.TryParse(text, memberMapData.TypeConverterOptions.NumberStyle.GetValueOrDefault(), memberMapData.TypeConverterOptions.CultureInfo, out int intResult))
			{
				return intResult;
			}

			return null;
		}
	}
}
