using Nest;
using System;

namespace ElasticsearchWithAnyDB.Models
{
	[ElasticsearchType(Name = "products")]
	public class Product
	{
		[Number(Name = nameof(Id))]
		public int Id { get; set; }

		[Keyword(Name = nameof(Name))]
		public string Name { get; set; }

		[Keyword(Name = nameof(Group))]
		public string Group { get; set; }

		[Number(Name = nameof(Price), Index = false)]
		public decimal Price { get; set; }

		[Number(Name = nameof(Amount), Index = false)]
		public decimal Amount { get; set; }

		[Date(Name = nameof(DateCreated))]
		public DateTime DateCreated { get; set; }

		[Nested(Name = nameof(BrandProduct))]
		public Brand BrandProduct { get; set; }
	}

	public class ProductMongo
	{
		public int Id { get; set; }
		public int ParentId { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public StatusProduct StatusProduct { get; set; }
		public Brand BrandProduct { get; set; }
		public string PhotoUrl { get; set; }
		public string PhotoUrlBig { get; set; }
		public string VideoUrl { get; set; }
		public string Description { get; set; }
		public string Keywords { get; set; }
		public string Availability { get; set; }
		public int? WholesalePacking { get; set; }
		public int? LimitOrderDays { get; set; }
	}
}
