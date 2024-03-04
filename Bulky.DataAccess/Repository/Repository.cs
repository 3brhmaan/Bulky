using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
	private readonly ApplicationDbContext _db;
	internal DbSet<T> dbSet;
	public Repository(ApplicationDbContext db)
	{
		_db = db;
		this.dbSet = _db.Set<T>();
	}
	public IEnumerable<T> GetAll()
	{
		return dbSet;
	}

	public T Get(Expression<Func<T, bool>> filter)
	{
		return dbSet.FirstOrDefault(filter);
	}

	public void Add(T entity)
	{
		dbSet.Add(entity);
	}

	public void Remove(T entity)
	{
		dbSet.Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entity)
	{
		dbSet.RemoveRange(entity);
	}
}