using CsvHelper.Configuration;
using OrderCalculator.Common;
using OrderCalculator.Framework.Culture;
using System;
using System.Linq;

namespace OrderCalculator.Csv.Config
{
	internal class CsvHelperConfig
	{
		public static Configuration GetConfiguration()
		{
			var configuration = new Configuration
			{
				CultureInfo		= CultureConfigurator.DefaultCulture,
				Delimiter		= Constants.Comma,
				HasHeaderRecord = false,
				TrimOptions		= TrimOptions.Trim
			};

			return configuration;
		}

		public static ClassMap GetMapping(Type genericType)
		{
			var classMapType = typeof(ClassMap<>);

			classMapType = classMapType.MakeGenericType(genericType);

			var type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).SingleOrDefault(x => classMapType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

			var mapping = type != null ? Activator.CreateInstance(type) as ClassMap : null;

			return mapping;
		}
	}
}
