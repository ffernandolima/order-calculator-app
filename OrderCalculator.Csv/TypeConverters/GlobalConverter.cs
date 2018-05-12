using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using OrderCalculator.Framework.Culture;
using System.Globalization;

namespace OrderCalculator.Csv.TypeConverters
{
	internal static class GlobalConverter
	{
		public static int? ConvertToNullInt(string source)
		{
			var converter = new NullIntConverter();

			var memberMapData = new MemberMapData(null)
			{
				TypeConverterOptions = new TypeConverterOptions
				{
					CultureInfo = CultureConfigurator.DefaultCulture,
					NumberStyle = NumberStyles.Integer
				}
			};

			return converter.ConvertFromString(source, null, memberMapData) as int?;
		}

		public static decimal? ConvertToNullDecimal(string source)
		{
			var converter = new NullDecimalConverter();

			var memberMapData = new MemberMapData(null)
			{
				TypeConverterOptions = new TypeConverterOptions
				{
					CultureInfo = CultureConfigurator.DefaultCulture,
					NumberStyle = NumberStyles.Number
				}
			};

			return converter.ConvertFromString(source, null, memberMapData) as decimal?;
		}
	}
}
