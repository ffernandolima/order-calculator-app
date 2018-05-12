using OrderCalculator.DataContracts;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace OrderCalculator.Data
{
	public class InMemoryReadonlyCsvUnitOfWork : IInMemoryReadonlyCsvUnitOfWork
	{
		private FileInfo _file;
		private ConcurrentDictionary<string, IInMemoryReadonlyCsvRepository> _repositories;

		public InMemoryReadonlyCsvUnitOfWork(FileInfo file)
		{
			this._file = file ?? throw new ArgumentNullException(nameof(file));
			this._repositories = new ConcurrentDictionary<string, IInMemoryReadonlyCsvRepository>();
		}

		public T CustomRepository<T>() where T : class
		{
			if (!typeof(T).IsInterface)
			{
				throw new ArgumentException("Generic type should be an interface.");
			}

			IInMemoryReadonlyCsvRepository factory(FileInfo file, Type type)
			{
				return (IInMemoryReadonlyCsvRepository)AppDomain.CurrentDomain.GetAssemblies()
																.SelectMany(t => t.GetTypes())
																.Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
																.Select(x => Activator.CreateInstance(x, file))
																.SingleOrDefault();
			}

			return (T)this.GetRepository(typeof(T), factory, "Custom");
		}

		public IInMemoryReadonlyCsvRepository<T> Repository<T>() where T : class
		{
			IInMemoryReadonlyCsvRepository factory(FileInfo file, Type type) => new InMemoryReadonlyCsvRepository<T>(file);

			return (IInMemoryReadonlyCsvRepository<T>)this.GetRepository(typeof(T), factory, "Generic");
		}

		private IInMemoryReadonlyCsvRepository GetRepository(Type objectType, Func<FileInfo, Type, IInMemoryReadonlyCsvRepository> repositoryFactory, string prefix)
		{
			var typeName = prefix + "." + objectType.FullName;

			if (!this._repositories.TryGetValue(typeName, out IInMemoryReadonlyCsvRepository repository))
			{
				repository = repositoryFactory.Invoke(this._file, objectType);

				this._repositories[typeName] = repository;
			}

			return repository;
		}

		#region IDisposable Members

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this._file = null;

					if (this._repositories != null)
					{
						foreach (var repository in this._repositories.Values)
						{
							repository.Dispose();
						}

						this._repositories.Clear();
						this._repositories = null;
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
