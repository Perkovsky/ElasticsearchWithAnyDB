using System;

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

		public void PrintError(string text)
		{
			var defaultTextColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(text);
			Console.ForegroundColor = defaultTextColor;
		}
	}
}
