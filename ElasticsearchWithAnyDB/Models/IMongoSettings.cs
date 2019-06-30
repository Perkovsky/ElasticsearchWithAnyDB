namespace ElasticsearchWithAnyDB.Models
{
	public interface IMongoSettings
	{
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
		string ProductsCollectionName { get; set; }
	}
}