using OrderCalculator.Models;
using System.Collections.Generic;

namespace OrderCalculator.DataContracts.Repositories
{
	public interface IInMemoryReadonlyCsvProductRepository : IInMemoryReadonlyCsvRepository<Product>
	{
		decimal Sum(IDictionary<string, int?> items);
	}
}
