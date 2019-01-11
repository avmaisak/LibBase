using System.Linq;

namespace LibBase.Data.Repository
{
	/// <summary>
	/// Repository that also implements IQueryable interface.
	/// </summary>
	/// <typeparam name="TEntity">The entity type that repository wraps.</typeparam>
	public interface IQueryableRepository<TEntity> : IRepository<TEntity>, IQueryable<TEntity>
		where TEntity : class
	{
	}
}
