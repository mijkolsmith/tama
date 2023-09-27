namespace Michaelotchi
{
	public interface IDataStore<T>
	{
		Task<bool> CreateItem(T item);
		Task<T> ReadItem(int id);
		Task<bool> UpdateItem(T item);
		Task<bool> DeleteItem(T item);
	}
}
