using Nest;
using System.ComponentModel.DataAnnotations;

namespace ElasticsearchWithAnyDB.Models
{
	public class Brand : IComparerById
	{
		[Key]
		[Number(Name = nameof(Id), Index = false, IgnoreMalformed = true, Coerce = true)]
		public int Id { get; set; }

		[Text(Name = nameof(Name))]
		public string Name { get; set; }
	}

	
}
