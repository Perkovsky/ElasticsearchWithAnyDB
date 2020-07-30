namespace ElasticsearchWithAnyDB.Models
{
	public interface IRedisSettings
	{
		string ConnectionString { get; set; }
	}
}