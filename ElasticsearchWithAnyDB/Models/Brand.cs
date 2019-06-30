using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nest;

namespace ElasticsearchWithAnyDB.Models
{
    public class Brand
    {
        [Ignore]
        [BsonIgnore]
        public int Id { get; set; } // for EF autoincrement

        [Number(Name = nameof(Id), Index = false, IgnoreMalformed = true, Coerce = true)]
        [BsonElement(nameof(Id))]
        public int Code1C { get; set; }

        [Text(Name = nameof(Name))]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType()) return false;

            Brand b = (Brand)obj;
            return (Code1C == b.Code1C);
        }

        public override int GetHashCode() => Code1C;
    }
}
