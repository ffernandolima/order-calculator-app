using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;

namespace OrderCalculator.Csv.TypeConverters
{
	public class NullDecimalConverter : DecimalConverter
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

			if (decimal.TryParse(text, memberMapData.TypeConverterOptions.NumberStyle.GetValueOrDefault(), memberMapData.TypeConverterOptions.CultureInfo, out decimal result))
			{
				return result;
			}

			return null;
		}
	}
}
