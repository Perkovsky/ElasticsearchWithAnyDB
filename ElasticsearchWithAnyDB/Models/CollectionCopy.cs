namespace ElasticsearchWithAnyDB.Models
{
	public class CollectionCopy
	{
		public string DbNameFrom { get; set; }
		public string CollectionNameFrom { get; set; }
		public string DbNameTo { get; set; }
		public string CollectionNameTo { get; set; }
	}
}
