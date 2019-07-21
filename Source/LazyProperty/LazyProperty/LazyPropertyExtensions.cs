using System;
using System.Runtime.CompilerServices;

namespace Studiotaiha.LazyProperty
{
	public static class LazyPropertyExtensions
	{
		public static TValue LazyProperty<TValue>(
			this ILazyPropertyHolder target,
			Func<TValue> valueProvider,
			[CallerMemberName]string propertyName = null)
		{
			if (valueProvider == null) { throw new ArgumentNullException(nameof(valueProvider)); }
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }

			return target.GetOrCreateValue(propertyName, valueProvider);
		}
	}
}
