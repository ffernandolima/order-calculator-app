using CsvHelper.Configuration;
using OrderCalculator.Csv.TypeConverters;
using OrderCalculator.Models;

namespace OrderCalculator.Csv.Mappings
{
	internal class ProductMap : ClassMap<Product>
	{
		public ProductMap()
		{
			this.Map(m => m.ProductId)
				.Index(0)
				.Default(default(string));

			this.Map(m => m.Stock)
				.Default(default(int?))
				.ConvertUsing(row => GlobalConverter.ConvertToNullInt(row.GetField(1)));

			this.Map(m => m.Price)
				.Default(default(decimal?))
				.ConvertUsing(row => GlobalConverter.ConvertToNullDecimal(row.GetField(2)));
		}
	}
}
