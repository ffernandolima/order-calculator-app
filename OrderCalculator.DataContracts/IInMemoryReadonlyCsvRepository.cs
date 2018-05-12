using System;
using System.Collections.Generic;

namespace OrderCalculator.DataContracts
{
	public interface IInMemoryReadonlyCsvRepository<T> : IInMemoryReadonlyCsvRepository where T : class
	{
		IList<T> ListAll();
	}

	public interface IInMemoryReadonlyCsvRepository : IDisposable
	{

	}
}
