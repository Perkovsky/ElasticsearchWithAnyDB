using ElasticsearchWithAnyDB.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace ElasticsearchWithAnyDB.Services
{
	public class PrintService : IPrintService
	{
		public void PrintInfo(string text, bool isTimestamp = true)
		{
			if (isTimestamp)
				Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss.fffff")}] - {text}");
			else
				Console.WriteLine(text);
		}

		public void PrintInfo(IEnumerable<Product> items)
		{
			Console.WriteLine("Item(s):");
			foreach (var item in items)
				Console.WriteLine($"\tID: {item.Id} \tParentID: {item.ParentId}   \tPrice: {item.Price} \tName: {HttpUtility.HtmlDecode(item.Name)}");
		}

		public void PrintInfo(IEnumerable<ProductSuggestion> items)
		{
			Console.WriteLine("Item(s):");
			foreach (var item in items)
				Console.WriteLine($"\tName: {HttpUtility.HtmlDecode(item.Name)}");
		}

		public void PrintError(string text)
		{
			var defaultTextColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(text);
			Console.ForegroundColor = defaultTextColor;
		}
	}
}
