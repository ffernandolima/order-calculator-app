using OrderCalculator.App.Configuration;
using OrderCalculator.App.Exceptions;
using OrderCalculator.Contracts.Services;
using OrderCalculator.Data;
using OrderCalculator.DataContracts;
using OrderCalculator.DataContracts.Repositories;
using System;

namespace OrderCalculator.App.Services
{
	public class OrderCalculatorService : IOrderCalculatorService
	{
		private readonly Settings _settings;

		private Argument _argument;
		private IInMemoryReadonlyCsvUnitOfWork _unitOfWork;

		public OrderCalculatorService(Argument argument)
		{
			this._argument = argument ?? throw new ArgumentNullException(nameof(argument));
			this._argument.EnsureValues();

			this._unitOfWork = InMemoryHelper.InMemoryReadonlyCsvUnitOfWork(this._argument.CatalogFile);
			this._settings = Settings.Instance;
		}

		public decimal? Calculate()
		{
			switch (this._argument.Operation)
			{
				case Operation.Unknown:
				default:
					throw new ArgumentException("No operation provided.");

				case Operation.CalculateOrder:
					{
						var totalPrice = this.CalculateOrder();

						return totalPrice;
					}
			}
		}

		private decimal CalculateOrder()
		{
			try
			{
				var totalPrice = this.GetTotalPrice();

				var totalTax = this._settings.ValueAddedTax * totalPrice;

				var totalPriceAddedTax = totalPrice + totalTax;

				return totalPriceAddedTax;
			}
			catch (Exception ex)
			{
				var ocex = new OrderCalculatorException(null, ex);

				throw ocex;
			}
		}

		private decimal GetTotalPrice()
		{
			using (var repository = this._unitOfWork.CustomRepository<IInMemoryReadonlyCsvProductRepository>())
			{
				var totalPrice = repository.Sum(this._argument.Items);

				return totalPrice;
			}
		}

		#region IDisposable Members

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					if (this._argument != null)
					{
						this._argument.Destroy();
						this._argument = null;
					}

					if (this._unitOfWork != null)
					{
						this._unitOfWork.Dispose();
						this._unitOfWork = null;
					}
				}
			}

			this._disposed = true;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion IDisposable Members
	}
}
