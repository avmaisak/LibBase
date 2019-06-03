namespace LibBase.Extensions
{
	public static class TypesExtensions
	{
		public static bool TypeEqual<T>(this object obj)
		{
			return obj.GetType() == typeof(T);
		}
	}
}
