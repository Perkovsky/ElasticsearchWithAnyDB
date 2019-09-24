namespace ElasticsearchWithAnyDB.Models
{
	public class MongoSettings : IMongoSettings
	{
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
		public string ProductsCollectionName { get; set; }
	}
}
