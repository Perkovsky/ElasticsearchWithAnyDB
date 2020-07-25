using System.Collections.Generic;

namespace ElasticsearchWithAnyDB.Models
{
	public interface IComparerById
	{
		int Id { get; set; }
	}

	public class ComparerById<T> : IEqualityComparer<T> where T : IComparerById
	{
		public bool Equals(T x, T y) => x.Id == y.Id;

		public int GetHashCode(T obj) => obj.Id;
	}
}
