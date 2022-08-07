namespace GuesserDB.DBBase.EntityRange;

public interface IEntityRange<T>
{
	public void AddRange(IEnumerable<T> range);
	public void SetRange(T[] range);
	public void Add(T entity);
	public T[] ToArray();
	public void Clear();
	void Save();
}