namespace LibBase.Entity
{
	/// <summary>
	/// Entity interface.
	/// </summary>
	/// <typeparam name="T">T.</typeparam>
	public interface IEntity<T>
	{
		/// <summary>
		/// Id.
		/// </summary>
		T Id { get; set; }
	}
}
