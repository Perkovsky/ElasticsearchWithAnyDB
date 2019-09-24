using ElasticsearchWithAnyDB.Models;
using System.Collections.Generic;

namespace ElasticsearchWithAnyDB.Services
{
	public interface IPrintService
	{
		void PrintInfo(IEnumerable<Product> items);
		void PrintInfo(IEnumerable<ProductSuggestion> items);
		void PrintInfo(string text, bool isTimestamp = true);
		void PrintError(string text);
	}
}