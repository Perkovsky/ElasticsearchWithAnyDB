using MongoDB.Bson.Serialization.Attributes;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ElasticsearchWithAnyDB.Models
{
    [ElasticsearchType(Name = "products")]
    public class Product
    {
        [JsonExtensionData]
        private IDictionary<string, JToken> additionalData;

        public Product() => additionalData = new Dictionary<string, JToken>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Number(Name = nameof(Id))]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "cat3_id")]
        [Keyword(Name = nameof(ParentID))]
        public int ParentID { get; set; }

        [Text(Name = nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "price")]
        [Number(Name = nameof(PriceB2B), Index = false)]
        public double PriceB2B { get; set; }

        [JsonProperty(PropertyName = "price_retail")]
        [Number(Name = nameof(PriceB2C), Index = false)]
        public double PriceB2C { get; set; }

        [JsonProperty(PropertyName = "wholesale_price")]
        [Number(Name = nameof(PriceWholesale), Index = false, IgnoreMalformed = true, Coerce = true)]
        public double? PriceWholesale { get; set; }

        [JsonProperty(PropertyName = "fold")]
        [Number(Name = nameof(WholesalePacking), Index = false)]
        public int WholesalePacking { get; set; }

        public StatusProduct StatusProduct { get; set; }

        [Ignore]
        [BsonIgnore]
        public int BrandProductId { get; set; }

        [Nested(Name = nameof(BrandProduct))]
        public Brand BrandProduct { get; set; }

        [JsonProperty(PropertyName = "video")]
        [Text(Name = nameof(VideoUrl), Index = false)]
        public string VideoUrl { get; set; }

        [Text(Name = nameof(Description))]
        public string Description { get; set; }

        [Text(Name = nameof(Keywords))]
        public string Keywords { get; set; }

        [Text(Name = nameof(Availability), Index = false)]
        public string Availability { get; set; }

        [Text(Name = nameof(Delivery), Index = false)]
        public string Delivery { get; set; }

        [JsonProperty(PropertyName = "limit_order_days")]
        [Number(Name = nameof(LimitOrderDays), Index = false, IgnoreMalformed = true, Coerce = true)]
        public int? LimitOrderDays { get; set; }

        [Number(Name = nameof(Weight), Index = false, IgnoreMalformed = true, Coerce = true)]
        public double? Weight { get; set; }

        [JsonProperty(PropertyName = "amount")]
        [Number(Name = nameof(Volume), Index = false, IgnoreMalformed = true, Coerce = true)]
        public double? Volume { get; set; }

        [JsonProperty(PropertyName = "amount_limit")]
        [Number(Name = nameof(VolumeLimit), Index = false, IgnoreMalformed = true, Coerce = true)]
        public double? VolumeLimit { get; set; }

        [JsonProperty(PropertyName = "amount_growth_limit")]
        [Number(Name = nameof(VolumeIncrementLimit), Index = false, IgnoreMalformed = true, Coerce = true)]
        public double? VolumeIncrementLimit { get; set; }

        [JsonProperty(PropertyName = "top100_old")]
        [Number(Name = nameof(PromoID), Index = false)]
        public int PromoID { get; set; }

        #region Only for Serialization ()

        //NOTE: This code is required to use property names for serialization, rather than [JsonProperty(PropertyName = "...")]
        //      Does not work for Elasticsearch, returns null values. Ignore private properties.

        //public bool ShouldSerializeParentID() => false;
        //public bool ShouldSerializePriceB2B() => false;
        //public bool ShouldSerializePriceB2C() => false;
        //public bool ShouldSerializePriceWholesale() => false;
        //public bool ShouldSerializeWholesalePacking() => false;
        //public bool ShouldSerializeVideoUrl() => false;
        //public bool ShouldSerializeLimitOrderDays() => false;
        //public bool ShouldSerializeVolume() => false;
        //public bool ShouldSerializeVolumeLimit() => false;
        //public bool ShouldSerializeVolumeIncrementLimit() => false;
        //public bool ShouldSerializePromoID() => false;

        //[JsonProperty(nameof(ParentID))]
        //private int ParentIDAlternateGetter => ParentID;
        //[JsonProperty(nameof(PriceB2B))]
        //private double PriceB2BAlternateGetter => PriceB2B;
        //[JsonProperty(nameof(PriceB2C))]
        //private double? PriceB2CAlternateGetter => PriceB2C;
        //[JsonProperty(nameof(PriceWholesale))]
        //private double? PriceWholesaleAlternateGetter => PriceWholesale;
        //[JsonProperty(nameof(WholesalePacking))]
        //private int WholesalePackingAlternateGetter => WholesalePacking;
        //[JsonProperty(nameof(VideoUrl))]
        //private string VideoUrlAlternateGetter => VideoUrl;
        //[JsonProperty(nameof(LimitOrderDays))]
        //private double? LimitOrderDaysAlternateGetter => LimitOrderDays;
        //[JsonProperty(nameof(Volume))]
        //private double? VolumeAlternateGetter => Volume;
        //[JsonProperty(nameof(VolumeLimit))]
        //private double? VolumeLimitAlternateGetter => VolumeLimit;
        //[JsonProperty(nameof(VolumeIncrementLimit))]
        //private double? VolumeIncrementLimitAlternateGetter => VolumeIncrementLimit;
        //[JsonProperty(nameof(PromoID))]
        //private int PromoIDAlternateGetter => PromoID;

        //[OnSerializing]
        //private void OnSerializingMethod(StreamingContext context) => additionalData = null;

        #endregion

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // only use for initial empty db
            if (additionalData.Count == 0) return;

            if (!string.IsNullOrEmpty((string)additionalData["brandId"]))
            {
                BrandProduct = new Brand
                {
                    Code1C = (int)additionalData["brandId"],
                    Name = (string)additionalData["brand"]
                };
            }
        }
    }
}
