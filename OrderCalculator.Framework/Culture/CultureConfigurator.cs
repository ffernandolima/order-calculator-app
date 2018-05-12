using System.Globalization;
using System.Threading;

namespace OrderCalculator.Framework.Culture
{
	public static class CultureConfigurator
	{
		public static CultureInfo DefaultCulture => CultureInfo.InvariantCulture;

		public static void Configure()
		{
			Thread.CurrentThread.CurrentCulture = DefaultCulture;
		}
	}
}
