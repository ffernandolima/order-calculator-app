using log4net.Config;
using OrderCalculator.Framework.Culture;
using OrderCalculator.Framework.Handlers;
using OrderCalculator.Framework.Logging;

namespace OrderCalculator.App
{
	internal class Startup
	{
		public static void Initialize()
		{
			// Culture
			CultureConfigurator.Configure();

			// Log4Net
			XmlConfigurator.Configure();

			// Log4Net
			Log4netHelper.LoggerPrefix = "OrderCalculator.App";

			// Exception Handler
			ExceptionHandler.Initialize();
		}
	}
}
