using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace LazyProperty
{
	public class PropertyBag : INotificationPropertyBag
	{
		[IgnoreDataMember]
		public IEnumerable<KeyValuePair<string, object>> Properties => this.Bag;

		[IgnoreDataMember]
		public int PropertiesCount => this.Bag.Count;

		[IgnoreDataMember]
		private IDictionary<string, object> Bag { get; } = new Dictionary<string, object>();

		public PropertyBag()
			: this(new Dictionary<string, object>())
		{
		}

		public PropertyBag(IDictionary<string, object> bag)
		{
			this.Bag = bag ?? throw new ArgumentNullException(nameof(bag));
		}

		public void ClearProperties(Func<string, object> defaultValueProvider = null)
		{
			var keys = this.Bag.Keys.ToArray();

			foreach (var key in keys)
			{
				this.DeleteProperty(
					key,
					defaultValueProvider: defaultValueProvider);
			}
		}

		public bool DeleteProperty<TValue>(
			string propertyName,
			Action<TValue, TValue> onDeleting = null,
			Action<TValue, TValue> onDeleted = null,
			Func<string, TValue> defaultValueProvider = null)
		{
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }

			if (this.TryGetValue<TValue>(propertyName, out var currentValue))
			{
				var defaultValue = defaultValueProvider == null
					? default
					: defaultValueProvider.Invoke(propertyName);

				onDeleting?.Invoke(currentValue, defaultValue);
				this.RaisePropertyChanging(propertyName);

				this.Bag.Remove(propertyName);

				onDeleted?.Invoke(currentValue, defaultValue);
				this.RaisePropertyChanged(propertyName);

				return true;
			}

			return false;
		}

		public TValue GetValue<TValue>(string propertyName)
		{
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }

			return this.Bag.TryGetValue(propertyName, out var boxedValue)
				? boxedValue is TValue castedValue
					? castedValue
					: (boxedValue == null)
						? default(TValue)
						: throw new InvalidCastException(propertyName)
				: throw new KeyNotFoundException(propertyName);
		}

		public bool SetValue<TValue>(
			string propertyName,
			TValue value,
			Action<TValue, TValue> onChanging = null,
			Action<TValue, TValue> onChanged = null,
			IEqualityComparer<TValue> equalityComparer = null,
			Func<string, TValue> defaultValueProvider = null)
		{
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }

			var valueExists = this.Bag.TryGetValue(propertyName, out var boxedvalue);

			var oldValue = valueExists && boxedvalue is TValue castedValue
				? castedValue
				: defaultValueProvider != null
					? defaultValueProvider.Invoke(propertyName)
					: default;

			var valueChanged = valueExists
				? !(equalityComparer ?? EqualityComparer<TValue>.Default).Equals(oldValue, value)
				: true;

			if (valueChanged)
			{
				onChanging?.Invoke(oldValue, value);
				this.RaisePropertyChanging(propertyName);

				this.Bag[propertyName] = value;

				onChanged?.Invoke(oldValue, value);
				this.RaisePropertyChanged(propertyName);
			}

			return valueChanged;
		}

		public bool TryGetValue<TValue>(string propertyName, out TValue value)
		{
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }

			if (this.Bag.TryGetValue(propertyName, out var boxedValue))
			{
				if (boxedValue is TValue castedValue)
				{
					value = castedValue;
					return true;
				}
				else if (boxedValue == (object)default(TValue))
				{
					value = default(TValue);
					return true;
				}
			}

			value = default;
			return false;
		}

		public TValue GetOrCreateValue<TValue>(
			string propertyName,
			Func<TValue> newValueProvider)
		{
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }
			if (newValueProvider == null) { throw new ArgumentNullException(nameof(newValueProvider)); }

			if (!this.TryGetValue<TValue>(propertyName, out var value))
			{
				value = newValueProvider.Invoke();
				this.SetValue(propertyName, value);
			}

			return value;
		}

		protected void RaisePropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void RaisePropertyChanging(string propertyName)
		{
			this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;
	}
}
