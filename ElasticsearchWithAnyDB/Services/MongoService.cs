using ElasticsearchWithAnyDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticsearchWithAnyDB.Services
{
	public class MongoService : IMongoService
	{
		private readonly IPrintService _printService;
		private readonly MongoClient _client;

		public MongoService(IMongoSettings settings, IPrintService printService)
		{
			_printService = printService;
			_client = new MongoClient(settings.ConnectionString);
		}

		public async Task CopyToAsync<T>(CollectionCopy copy)
		{
			var dbNameFrom = copy.DbNameFrom;
			var dbNameTo = copy.DbNameTo;
			var collectionNameFrom = copy.CollectionNameFrom;
			var collectionNameTo = copy.CollectionNameTo;

			_printService.PrintInfo($"Started copying from '{dbNameFrom}->{collectionNameFrom}' to '{dbNameTo}->{collectionNameTo}'...");

			var dbFrom = _client.GetDatabase(dbNameFrom);
			var dbTo = _client.GetDatabase(dbNameTo);
			var collectionFrom = dbFrom.GetCollection<T>(collectionNameFrom);
			var collectionTo = dbTo.GetCollection<T>(collectionNameTo);

			var result = await collectionFrom.FindAsync(x => true);
			await collectionTo.InsertManyAsync(await result.ToListAsync());
		}

		public async Task<long> CountAsync<T>(string dbName, string collectionName)
		{
			var db = _client.GetDatabase(dbName);
			var collection = db.GetCollection<T>(collectionName);
			return await collection.CountDocumentsAsync(x => true);
		}
	}
}
