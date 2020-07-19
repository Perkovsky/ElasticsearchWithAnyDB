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
}
