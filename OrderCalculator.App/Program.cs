using OrderCalculator.App.Services;
using OrderCalculator.Contracts.Services;

namespace OrderCalculator.App
{
	public class Program
	{
		public static int Main(string[] args)
		{
			Startup.Initialize();

			using (IOrderCalculatorController controller = new OrderCalculatorController(args))
			{
				var result = controller.Execute();

				return result;
			}
		}
	}
}
