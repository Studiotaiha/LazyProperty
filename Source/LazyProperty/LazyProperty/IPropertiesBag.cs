using System;
using System.Collections.Generic;

namespace Studiotaiha.LazyProperty
{
	public interface IPropertyBag : ILazyPropertyHolder
	{
		IEnumerable<KeyValuePair<string, object>> Properties { get; }

		int PropertiesCount { get; }

		void ClearProperties(Func<string, object> defaultValueProvider = null);

		bool DeleteProperty<TValue>(
			string propertyName,
			Action<TValue, TValue> onDeleting = null,
			Action<TValue, TValue> onDeleted = null,
			Func<string, TValue> defaultValueProvider = null);

		TValue GetValue<TValue>(string propertyName);

		bool SetValue<TValue>(
			string propertyName,
			TValue value,
			Action<TValue, TValue> onChanging = null,
			Action<TValue, TValue> onChanged = null,
			IEqualityComparer<TValue> equalityComparer = null,
			Func<string, TValue> defaultValueProvider = null);

		bool TryGetValue<TValue>(
			string propertyName,
			out TValue value);
	}
}
