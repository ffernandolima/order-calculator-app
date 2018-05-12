using System;

namespace OrderCalculator.Models
{
	[Serializable]
	public class Product
	{
		public string ProductId { get; set; }
		public int? Stock { get; set; }
		public decimal? Price { get; set; }
	}
}
