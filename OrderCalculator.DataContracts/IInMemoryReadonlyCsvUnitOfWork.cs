using System;

namespace OrderCalculator.DataContracts
{
	public interface IInMemoryReadonlyCsvUnitOfWork : IDisposable
	{
		T CustomRepository<T>() where T : class;
		IInMemoryReadonlyCsvRepository<T> Repository<T>() where T : class;
	}
}
