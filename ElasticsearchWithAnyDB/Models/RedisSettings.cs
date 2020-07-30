namespace ElasticsearchWithAnyDB.Models
{
	public class RedisSettings : IRedisSettings
	{
		public string ConnectionString { get; set; }
	}
}
