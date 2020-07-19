namespace ElasticsearchWithAnyDB.Services
{
	public interface IPrintService
	{
		void PrintInfo(string text, bool isTimestamp = true);
		void PrintError(string text);
	}
}