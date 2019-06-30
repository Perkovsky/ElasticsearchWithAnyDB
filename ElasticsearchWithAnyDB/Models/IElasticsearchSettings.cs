namespace ElasticsearchWithAnyDB.Models
{
	public interface IElasticsearchSettings
	{
		string ConnectionString { get; set; }
		string IndexName { get; set; }
	}
}