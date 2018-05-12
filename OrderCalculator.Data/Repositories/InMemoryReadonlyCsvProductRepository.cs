using OrderCalculator.Data.Exceptions;
using OrderCalculator.DataContracts.Repositories;
using OrderCalculator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace OrderCalculator.Data.Repositories
{
	public class InMemoryReadonlyCsvProductRepository : InMemoryReadonlyCsvRepository<Product>, IInMemoryReadonlyCsvProductRepository
	{
		public InMemoryReadonlyCsvProductRepository(FileInfo file)
			: base(file)
		{
			this.RequiredValidators = new List<Func<Product, object>>
			{
				x => x.ProductId,
				x => x.Stock,
				x => x.Price
			};
		}

		public decimal Sum(IDictionary<string, int?> items)
		{
			decimal totalPrice = 0;

			var ids = items.Select(x => x.Key).ToArray();

			var resultSet = this.InMemorySet.Where(x => ids.Contains(x.ProductId, StringComparer.OrdinalIgnoreCase)).ToArray();

			foreach (var item in items)
			{
				var products = resultSet.Where(x => x.ProductId.Equals(item.Key, StringComparison.OrdinalIgnoreCase)).ToArray();

				if (products == null || !products.Any())
				{
					throw new DataValidatorException($"Product {item.Key} doesn't exist.");
				}

				if (products.Length > 1)
				{
					throw new DataValidatorException($"There are {products.Length} products which have the same id: {item.Key}.");
				}

				var product = products.First();

				if (!product.Stock.HasValue || product.Stock <= 0 || product.Stock < item.Value)
				{
					throw new DataValidatorException($"Product {item.Key} is out of stock.");
				}

				if (!product.Price.HasValue || product.Price <= 0)
				{
					throw new DataValidatorException($"Product {item.Key} doesn't have price.");
				}

				var price = (item.Value * product.Price).GetValueOrDefault();

				totalPrice += price;
			}

			return totalPrice;
		}

		#region IDisposable Members

		private bool _disposed;

		protected override void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					base.Dispose(true);
				}
			}

			this._disposed = true;
		}

		#endregion
	}
}
