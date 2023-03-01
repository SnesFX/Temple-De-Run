using System.Collections.Generic;

public static class Ext
{
	public static bool IsAnyOf<T>(this T t, params T[] values)
	{
		for (int i = 0; i < values.Length; i++)
		{
			if (EqualityComparer<T>.Default.Equals(values[i], t))
			{
				return true;
			}
		}
		return false;
	}
}
