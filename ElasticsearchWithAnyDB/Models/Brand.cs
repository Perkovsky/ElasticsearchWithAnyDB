using Nest;

namespace ElasticsearchWithAnyDB.Models
{
	public class Brand
	{
		[Number(Name = nameof(Id), Index = false, IgnoreMalformed = true, Coerce = true)]
		public int Id { get; set; }

		[Text(Name = nameof(Name))]
		public string Name { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null || this.GetType() != obj.GetType()) return false;

			Brand b = (Brand)obj;
			return (Id == b.Id);
		}

		public override int GetHashCode() => Id;
	}
}
