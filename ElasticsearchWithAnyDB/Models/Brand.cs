using Nest;

namespace ElasticsearchWithAnyDB.Models
{
	public class Brand
	{
		[Number(Name = nameof(Id), Index = false, IgnoreMalformed = true, Coerce = true)]
		public int Id { get; set; }

		[Text(Name = nameof(Name))]
		public string Name { get; set; }
	}
}
