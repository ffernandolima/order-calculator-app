using OrderCalculator.DataContracts;
using System.IO;

namespace OrderCalculator.Data
{
	public static class InMemoryHelper
	{
		public static IInMemoryReadonlyCsvUnitOfWork InMemoryReadonlyCsvUnitOfWork(FileInfo file)
		{
			var unitOfWork = new InMemoryReadonlyCsvUnitOfWork(file);

			return unitOfWork;
		}
	}
}
