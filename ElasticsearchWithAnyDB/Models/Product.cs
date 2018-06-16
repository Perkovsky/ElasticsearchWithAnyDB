using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ElasticsearchWithAnyDB.Models
{
    public class Product
    {
        [JsonExtensionData]
        private IDictionary<string, JToken> additionalData;

        public Product() => additionalData = new Dictionary<string, JToken>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "cat3_id")]
        public int ParentID { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "price")]
        public double PriceB2B { get; set; }
        [JsonProperty(PropertyName = "price_retail")]
        public double PriceB2C { get; set; }
        [JsonProperty(PropertyName = "wholesale_price")]
        public double? PriceWholesale { get; set; }

        [JsonProperty(PropertyName = "fold")]
        public int WholesalePacking { get; set; }
        public StatusProduct StatusProduct { get; set; }
        public int BrandProductId { get; set; }
        public Brand BrandProduct { get; set; }
        [JsonProperty(PropertyName = "video")]
        public string VideoUrl { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Availability { get; set; }
        public string Delivery { get; set; }
        [JsonProperty(PropertyName = "limit_order_days")]
        public int? LimitOrderDays { get; set; }
        public double? Weight { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public double? Volume { get; set; }
        [JsonProperty(PropertyName = "amount_limit")]
        public double? VolumeLimit { get; set; }
        [JsonProperty(PropertyName = "amount_growth_limit")]
        public double? VolumeIncrementLimit { get; set; }
        [JsonProperty(PropertyName = "top100_old")]
        public int PromoID { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // only use for initial empty db
            if (additionalData.Count == 0) return;

            //if (!string.IsNullOrEmpty((string)additionalData["brandId"]))
            //{
            //    BrandProduct = new Brand
            //    {
            //        Code1C = (int)additionalData["brandId"],
            //        Name = (string)additionalData["brand"]
            //    };
            //}
        }
    }
}
