using Nest;

namespace ElasticsearchWithAnyDB.Models
{
    [ElasticsearchType(Name = "products")]
    public class Product
    {
        [Number(Name = nameof(Id))]
        public int Id { get; set; }

        [Keyword(Name = nameof(ParentId))]
        public int ParentId { get; set; }

        [Text(Name = nameof(Name))]
        public string Name { get; set; }

        [Number(Name = nameof(Price), Index = false)]
        public double Price { get; set; }

        public StatusProduct StatusProduct { get; set; }

        [Nested(Name = nameof(BrandProduct))]
        public Brand BrandProduct { get; set; }

		[Text(Name = nameof(PhotoUrl), Index = false)]
		public string PhotoUrl { get; set; }

		[Text(Name = nameof(PhotoUrlBig), Index = false)]
		public string PhotoUrlBig { get; set; }

        [Text(Name = nameof(VideoUrl), Index = false)]
        public string VideoUrl { get; set; }

        [Text(Name = nameof(Description))]
        public string Description { get; set; }

        [Text(Name = nameof(Keywords))]
        public string Keywords { get; set; }

        [Text(Name = nameof(Availability), Index = false)]
        public string Availability { get; set; }

        [Number(Name = nameof(WholesalePacking), Index = false, IgnoreMalformed = true, Coerce = true)]
        public int? WholesalePacking { get; set; }

		[Number(Name = nameof(LimitOrderDays), Index = false, IgnoreMalformed = true, Coerce = true)]
        public int? LimitOrderDays { get; set; }
    }
}
