using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticsearchWithAnyDB.Models
{
	public class GroupMongo
	{
		public int Id { get; set; }
		public int? ParentId { get; set; }
		public string Name { get; set; }
		public string Keywords { get; set; }
		public string PhotoUrl { get; set; }
	}

	public class Group
	{
		[Key]
		public int Id { get; set; }

		public int? ParentId { get; set; }

		[ForeignKey(nameof(ParentId))]
		public Group Parent { get; set; }

		public string Name { get; set; }
		public string Keywords { get; set; }
		public string PhotoUrl { get; set; }
	}
}
