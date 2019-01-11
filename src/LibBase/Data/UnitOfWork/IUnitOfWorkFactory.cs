
namespace LibBase.Data.UnitOfWork
{
	/// <summary>
	/// Unit of work factory abstraction.
	/// </summary>
	public interface IUnitOfWorkFactory<out TUnitOfWork> where TUnitOfWork : class
	{
		/// <summary>
		/// Creates unit of work with default isolation level.
		/// </summary>
		/// <returns>Unit of work.</returns>
		TUnitOfWork Create();
	}
}
