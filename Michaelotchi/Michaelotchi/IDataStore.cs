namespace Michaelotchi
{
	public interface IDataStore<T>
	{
		bool CreateItem(T item);
		T ReadItem();
		bool UpdateItem(T item);
		bool DeleteItem(T item);
	}
}
