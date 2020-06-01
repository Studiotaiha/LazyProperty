using System;

namespace LazyProperty
{
	public interface ILazyPropertyHolder
	{
		public TValue GetOrCreateValue<TValue>(
			string propertyName,
			Func<TValue> valueProvider);
	}
}
