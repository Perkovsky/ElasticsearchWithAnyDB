namespace ElasticsearchWithAnyDB.Models
{
	public class ElasticsearchSettings : IElasticsearchSettings
	{
		public string ConnectionString { get; set; }
		public string IndexName { get; set; }
	}
}
