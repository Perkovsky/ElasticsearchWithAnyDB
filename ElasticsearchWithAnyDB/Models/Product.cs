using Nest;
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
        [Keyword(Name = nameof(ParentID))]
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

        #region Only for Serialization

        //NOTE: This code is required to use property names for serialization, rather than [JsonProperty(PropertyName = "...")]

        public bool ShouldSerializeParentID() => false;
        public bool ShouldSerializePriceB2B() => false;
        public bool ShouldSerializePriceB2C() => false;
        public bool ShouldSerializePriceWholesale() => false;
        public bool ShouldSerializeWholesalePacking() => false;
        public bool ShouldSerializeVideoUrl() => false;
        public bool ShouldSerializeLimitOrderDays() => false;
        public bool ShouldSerializeVolume() => false;
        public bool ShouldSerializeVolumeLimit() => false;
        public bool ShouldSerializeVolumeIncrementLimit() => false;
        public bool ShouldSerializePromoID() => false;
       
        [JsonProperty(nameof(ParentID))]
        private int ParentIDAlternateGetter => ParentID;
        [JsonProperty(nameof(PriceB2B))]
        private double PriceB2BAlternateGetter => PriceB2B;
        [JsonProperty(nameof(PriceB2C))]
        private double? PriceB2CAlternateGetter => PriceB2C;
        [JsonProperty(nameof(PriceWholesale))]
        private double? PriceWholesaleAlternateGetter => PriceWholesale;
        [JsonProperty(nameof(WholesalePacking))]
        private int WholesalePackingAlternateGetter => WholesalePacking;
        [JsonProperty(nameof(VideoUrl))]
        private string VideoUrlAlternateGetter => VideoUrl;
        [JsonProperty(nameof(LimitOrderDays))]
        private double? LimitOrderDaysAlternateGetter => LimitOrderDays;
        [JsonProperty(nameof(Volume))]
        private double? VolumeAlternateGetter => Volume;
        [JsonProperty(nameof(VolumeLimit))]
        private double? VolumeLimitAlternateGetter => VolumeLimit;
        [JsonProperty(nameof(VolumeIncrementLimit))]
        private double? VolumeIncrementLimitAlternateGetter => VolumeIncrementLimit;
        [JsonProperty(nameof(PromoID))]
        private int PromoIDAlternateGetter => PromoID;

        [OnSerializing]
        private void OnSerializingMethod(StreamingContext context) => additionalData = null;

        #endregion

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
