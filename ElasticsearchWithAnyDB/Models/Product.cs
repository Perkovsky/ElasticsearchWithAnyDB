namespace ElasticsearchWithAnyDB.Models
{
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
