using OrderCalculator.Csv.Factory;
using OrderCalculator.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrderCalculator.Data
{
	public class InMemoryReadonlyCsvRepository<T> : InMemoryReadonlyCsvRepository, IInMemoryReadonlyCsvRepository<T> where T : class
	{
		private IList<T> _inMemorySet;

		protected IList<T> InMemorySet => this.GetInMemorySet();
		public IList<Func<T, object>> RequiredValidators { get; set; }

		public InMemoryReadonlyCsvRepository(FileInfo file)
			: base(file)
		{
			this._inMemorySet = null;
			this.RequiredValidators = null;
		}

		public IList<T> ListAll()
		{
			return this.InMemorySet;
		}

		private IList<T> GetInMemorySet()
		{
			if (this._inMemorySet == null)
			{
				this._inMemorySet = new List<T>(this.LoadInMemorySet());

				this.ValidateInMemorySet();
			}

			return this._inMemorySet;
		}

		private IEnumerable<T> LoadInMemorySet()
		{
			using (var reader = CsvReaderFactory.GetReader(this.File, typeof(T)))
			{
				var records = reader.GetRecords(typeof(T)).Cast<T>().ToArray();

				return records;
			}
		}

		private void ValidateInMemorySet()
		{
			if (this.RequiredValidators != null && this.RequiredValidators.Any())
			{
				foreach (var validator in this.RequiredValidators)
				{
					var enumerable = this._inMemorySet.Select(validator);

					var doesItHaveNullValues = enumerable.Any(value => value == null);

					if (doesItHaveNullValues)
					{
						throw new ArgumentException("In memory set hasn't been built properly. Please open the file and check the values out.");
					}
				}
			}
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

	public class InMemoryReadonlyCsvRepository : IInMemoryReadonlyCsvRepository
	{
		protected FileInfo File { get; private set; }

		public InMemoryReadonlyCsvRepository(FileInfo file)
		{
			this.File = file ?? throw new ArgumentNullException(nameof(file));
		}

		#region IDisposable Members

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{

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
