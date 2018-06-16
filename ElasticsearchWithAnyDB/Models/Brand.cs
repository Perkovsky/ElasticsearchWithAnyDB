namespace ElasticsearchWithAnyDB.Models
{
    public class Brand
    {
        public int Id { get; set; } // for EF autoincrement
        public string Name { get; set; }
        public int Code1C { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType()) return false;

            Brand b = (Brand)obj;
            return (Code1C == b.Code1C);
        }

        public override int GetHashCode() => Code1C;
    }
}
