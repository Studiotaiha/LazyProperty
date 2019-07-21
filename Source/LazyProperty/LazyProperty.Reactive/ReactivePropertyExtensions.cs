using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Studiotaiha.LazyProperty
{
	public interface IReactiveLazyPropertyHolder : ILazyPropertyHolder, IDisposable
	{
		public ICollection<IDisposable> Disposables { get; }
	}

	public static class ReactivePropertyExtensions
	{
		public static IObservable<TValue> PropertyChangedAsObesrvable<TSource, TValue>(
			this TSource source,
			string propertyName,
			Func<TSource, TValue> propertyValueProvider)
			where TSource : INotifyPropertyChanged
		{
			return source
				.PropertyChangedAsObservable(propertyName)
				.Select(x => propertyValueProvider.Invoke(source));
		}

		public static IObservable<TValue> PropertyChangedAsObesrvable<TSource, TValue>(
			this TSource source,
			Expression<Func<TSource, TValue>> propertySelector)
			where TSource : INotifyPropertyChanged
		{
			if (propertySelector == null) { throw new ArgumentNullException(nameof(propertySelector)); }

			var propertyName = ((MemberExpression)propertySelector.Body).Member.Name;
			return source.PropertyChangedAsObesrvable(
				propertyName,
				propertySelector.Compile());
		}

		public static IObservable<PropertyChangedEventArgs> PropertyChangedAsObservable(
			this INotifyPropertyChanged notifyPropertyChanged,
			string propertyName)
		{
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }

			return notifyPropertyChanged
				.PropertyChangedAsObservable()
				.Where(x => x.PropertyName == propertyName);
		}

		public static ReadOnlyReactiveProperty<TValue> ToReadOnlyReactiveProperty<TSource, TValue>(
			this TSource source,
			string propertyName,
			Func<TSource, TValue> propertyValueProvider)
			where TSource : INotifyPropertyChanged
		{
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }
			if (propertyValueProvider == null) { throw new ArgumentNullException(nameof(propertyValueProvider)); }

			return source.ToReadOnlyReactiveProperty(
				propertyName,
				propertyValueProvider,
				x => x);
		}

		public static ReadOnlyReactiveProperty<TConverted> ToReadOnlyReactiveProperty<TSource, TValue, TConverted>(
			this TSource source,
			string propertyName,
			Func<TSource, TValue> propertyValueProvider,
			Func<TValue, TConverted> valueConverter)
			where TSource : INotifyPropertyChanged
		{
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }
			if (propertyValueProvider == null) { throw new ArgumentNullException(nameof(propertyValueProvider)); }

			return source
				.PropertyChangedAsObservable(propertyName)
				.Select(x => propertyValueProvider.Invoke(source))
				.Select(x => valueConverter.Invoke(x))
				.ToReadOnlyReactiveProperty(valueConverter.Invoke(propertyValueProvider.Invoke(source)));
		}

		public static ReadOnlyReactiveProperty<TValue> ToReadOnlyReactiveProperty<TSource, TValue>(
			this TSource source,
			Expression<Func<TSource, TValue>> propertySelector)
			where TSource : INotifyPropertyChanged
		{
			if (propertySelector == null) { throw new ArgumentNullException(nameof(propertySelector)); }

			return source.ToReadOnlyReactiveProperty(
				propertySelector,
				x => x);
		}

		public static ReadOnlyReactiveProperty<TConverted> ToReadOnlyReactiveProperty<TSource, TValue, TConverted>(
			this TSource source,
			Expression<Func<TSource, TValue>> propertySelector,
			Func<TValue, TConverted> valueConverter)
			where TSource : INotifyPropertyChanged
		{
			if (propertySelector == null) { throw new ArgumentNullException(nameof(propertySelector)); }

			var propertyName = ((MemberExpression)propertySelector.Body).Member.Name;
			return source.ToReadOnlyReactiveProperty(
				propertyName,
				propertySelector.Compile(),
				valueConverter);
		}

		public static ReadOnlyReactiveProperty<TConverted> ToReadOnlyReactiveProperty<TSource, TValue, TConverted>(
			this TSource source,
			TimeSpan interval,
			string propertyName,
			Func<TSource, TValue> propertyValueProvider,
			Func<TValue, TConverted> valueConverter)
			where TSource : INotifyPropertyChanged
		{
			if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }
			if (propertyValueProvider == null) { throw new ArgumentNullException(nameof(propertyValueProvider)); }

			return source
				.PropertyChangedAsObservable(propertyName)
				.Window(interval)
				.SelectMany(x => x)
				.Select(x => propertyValueProvider.Invoke(source))
				.Select(x => valueConverter.Invoke(x))
				.ToReadOnlyReactiveProperty(valueConverter.Invoke(propertyValueProvider.Invoke(source)));
		}

		public static ReadOnlyReactiveProperty<TValue> ToReadOnlyReactiveProperty<TSource, TValue>(
			this TSource source,
			TimeSpan interval,
			Expression<Func<TSource, TValue>> propertySelector)
			where TSource : INotifyPropertyChanged
		{
			if (propertySelector == null) { throw new ArgumentNullException(nameof(propertySelector)); }

			return source.ToReadOnlyReactiveProperty(
				interval,
				propertySelector,
				x => x);
		}

		public static ReadOnlyReactiveProperty<TConverted> ToReadOnlyReactiveProperty<TSource, TValue, TConverted>(
			this TSource source,
			TimeSpan interval,
			Expression<Func<TSource, TValue>> propertySelector,
			Func<TValue, TConverted> valueConverter)
			where TSource : INotifyPropertyChanged
		{
			if (propertySelector == null) { throw new ArgumentNullException(nameof(propertySelector)); }

			var propertyName = ((MemberExpression)propertySelector.Body).Member.Name;
			return source.ToReadOnlyReactiveProperty(
				interval,
				propertyName,
				propertySelector.Compile(),
				valueConverter);
		}
	}
}
