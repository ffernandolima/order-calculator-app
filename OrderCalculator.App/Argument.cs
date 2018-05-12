using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrderCalculator.App
{
	public class Argument
	{
		public Operation? Operation { get; private set; }
		public FileInfo CatalogFile { get; private set; }
		public IDictionary<string, int?> Items { get; private set; }

		public Argument(Operation? operation, FileInfo catalogFile, IDictionary<string, int?> items)
		{
			this.Operation = operation;
			this.CatalogFile = catalogFile;
			this.Items = items;
		}

		public void EnsureValues()
		{
			if (!this.Operation.HasValue || this.Operation.Value == App.Operation.Unknown)
			{
				throw new ArgumentException(nameof(this.Operation));
			}

			if (this.CatalogFile == null || !this.CatalogFile.Exists)
			{
				throw new ArgumentException(nameof(this.CatalogFile));
			}

			if (this.Items == null || !this.Items.Any())
			{
				throw new ArgumentException(nameof(this.Items));
			}
		}

		public void Destroy()
		{
			this.Operation = null;
			this.CatalogFile = null;

			if (this.Items != null && this.Items.Any())
			{
				this.Items.Clear();
				this.Items = null;
			}
		}
	}
}
