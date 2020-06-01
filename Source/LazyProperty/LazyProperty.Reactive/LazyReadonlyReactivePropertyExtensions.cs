using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace LazyProperty
{
	public static class LazyReadonlyReactivePropertyExtensions
	{
		public static ReadOnlyReactiveProperty<T> LazyReadOnlyReactiveProperty<T>(
			this IReactiveLazyPropertyHolder target,
			Func<ReadOnlyReactiveProperty<T>> valueProvider,
			[CallerMemberName]string propertyName = null)
		{
			return target
				.LazyProperty(valueProvider, propertyName)
				.AddTo(target.Disposables);
		}

		public static ReadOnlyReactiveProperty<TValue> LazyReadOnlyReactivePropertyFrom<TSource, TValue>(
			this IReactiveLazyPropertyHolder target,
			TSource source,
			Expression<Func<TSource, TValue>> propertySelector,
			[CallerMemberName]string propertyName = null)
			where TSource : INotifyPropertyChanged
		{
			ReadOnlyReactiveProperty<TValue> createReactiveProperty()
			{
				return source.ToReadOnlyReactiveProperty(propertySelector);
			}

			return target.LazyReadOnlyReactiveProperty(createReactiveProperty, propertyName);
		}

		public static ReadOnlyReactiveProperty<TConverted> LazyReadOnlyReactivePropertyFrom<TSource, TValue, TConverted>(
			this IReactiveLazyPropertyHolder target,
			TSource source,
			Expression<Func<TSource, TValue>> propertySelector,
			Func<TValue, TConverted> valueConverter,
			[CallerMemberName]string propertyName = null)
			where TSource : INotifyPropertyChanged
		{
			ReadOnlyReactiveProperty<TConverted> createReactiveProperty()
			{
				return source.ToReadOnlyReactiveProperty(propertySelector, valueConverter);
			}

			return target.LazyReadOnlyReactiveProperty(
				createReactiveProperty,
				propertyName);
		}

		public static ReadOnlyReactiveProperty<TValue> LazyReadOnlyReactivePropertyFrom<TSource, TValue>(
			this IReactiveLazyPropertyHolder target,
			TimeSpan interval,
			TSource source,
			Expression<Func<TSource, TValue>> propertySelector,
			[CallerMemberName]string propertyName = null)
			where TSource : INotifyPropertyChanged
		{
			ReadOnlyReactiveProperty<TValue> createReactiveProperty()
			{
				return source.ToReadOnlyReactiveProperty(interval, propertySelector);
			}

			return target.LazyReadOnlyReactiveProperty(
				createReactiveProperty,
				propertyName);
		}

		public static ReadOnlyReactiveProperty<TConverted> LazyReadOnlyReactivePropertyFrom<TSource, TValue, TConverted>(
			this IReactiveLazyPropertyHolder target,
			TimeSpan interval,
			TSource source,
			Expression<Func<TSource, TValue>> propertySelector,
			Func<TValue, TConverted> valueConverter,
			[CallerMemberName]string propertyName = null)
			where TSource : INotifyPropertyChanged
		{
			ReadOnlyReactiveProperty<TConverted> createReactiveProperty()
			{
				return source.ToReadOnlyReactiveProperty(interval, propertySelector, valueConverter);
			}

			return target.LazyReadOnlyReactiveProperty(
				createReactiveProperty,
				propertyName);
		}
	}
}
