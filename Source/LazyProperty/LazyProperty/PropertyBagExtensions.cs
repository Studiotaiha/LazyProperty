using System;
using System.Collections.Generic;

namespace Studiotaiha.LazyProperty
{

	public static class PropertyBagExtensions
	{
		public static bool DeleteProperty(
			this IPropertyBag propertyBag,
			string propertyName,
			Action<object, object> onDeleting = null,
			Action<object, object> onDeleted = null,
			Func<string, object> defaultValueProvider = null)
		{
			return propertyBag.DeleteProperty(
				propertyName,
				onDeleting,
				onDeleted,
				defaultValueProvider);
		}

		public static object GetValueAsObject(
					this IPropertyBag propertyBag,
			string propertyName)
		{
			return propertyBag.GetValue<object>(propertyName);
		}

		public static bool SetValueAsObject(
			this IPropertyBag propertyBag,
			string propertyName,
			object value,
			Action<object, object> onChanging = null,
			Action<object, object> onChanged = null,
			IEqualityComparer<object> equalityComparer = null,
			Func<string, object> defaultValueProvider = null)
		{
			return propertyBag.SetValue<object>(
				propertyName,
				value,
				onChanging,
				onChanged,
				equalityComparer,
				defaultValueProvider);
		}

		public static bool TryGetValueAsObject(
			this IPropertyBag propertyBag,
			string propertyName,
			out object value)
		{
			return propertyBag.TryGetValue<object>(propertyName, out value);
		}

		#region GetValue

		public static TValue GetValueOrDefault<TValue>(
			this IPropertyBag propertyBag,
			string propertyName,
			Func<string, TValue> defaultValueProvider)
		{
			if (defaultValueProvider == null) { throw new ArgumentNullException(nameof(defaultValueProvider)); }

			return propertyBag.TryGetValue<TValue>(propertyName, out var value)
				? value
				: defaultValueProvider.Invoke(propertyName);
		}

		public static TValue GetValueOrDefault<TValue>(
			this IPropertyBag propertyBag,
			string propertyName,
			Func<TValue> defaultValueProvider)
		{
			if (defaultValueProvider == null) { throw new ArgumentNullException(nameof(defaultValueProvider)); }

			return propertyBag.TryGetValue<TValue>(propertyName, out var value)
				? value
				: defaultValueProvider.Invoke();
		}

		public static TValue GetValueOrDefault<TValue>(
			this IPropertyBag propertyBag,
			string propertyName,
			TValue defaultValue)
		{
			return propertyBag.TryGetValue<TValue>(propertyName, out var value)
				? value
				: defaultValue;
		}

		#endregion GetValue

		#region GetOrCreateValue

		public static TValue GetOrCreateValue<TValue>(
			this IPropertyBag propertyBag,
			string propertyName,
			Func<TValue> newValueProvider,
			Action<TValue> onCreated = null)
		{
			if (newValueProvider == null) { throw new ArgumentNullException(nameof(newValueProvider)); }

			TValue actualNewValueProvider()
			{
				return newValueProvider.Invoke();
			};

			return propertyBag.GetOrCreateValue(
				propertyName,
				actualNewValueProvider,
				onCreated);
		}

		public static TValue GetOrCreateValue<TValue>(
			this IPropertyBag propertyBag,
			string propertyName,
			TValue newValue,
			Action<TValue> onCreated = null)
		{
			TValue actualNewValueProvider()
			{
				return newValue;
			};

			return propertyBag.GetOrCreateValue(
				propertyName,
				actualNewValueProvider,
				onCreated);
		}

		#endregion GetOrCreateValue
	}
}
