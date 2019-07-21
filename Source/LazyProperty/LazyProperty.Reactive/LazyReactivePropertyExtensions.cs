using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Studiotaiha.LazyProperty
{
	public static class LazyReactivePropertyExtensions
	{
		public static ReactiveProperty<T> LazyReactiveProperty<T>(
			this IReactiveLazyPropertyHolder target,
			Func<ReactiveProperty<T>> valueProvider,
			[CallerMemberName]string propertyName = null)
		{
			return target
				.LazyProperty(valueProvider, propertyName)
				.AddTo(target.Disposables);
		}

		#region with InitialValueProvider

		//public static ReactiveProperty<T> LazyReactiveProperty<T>(
		//	this IReactiveLazyPropertyHolder target,
		//	Func<T> initialValueProvider,
		//	ReactivePropertyMode mode = ReactivePropertyMode.Default,
		//	IEqualityComparer<T> equalityComparer = null,
		//	[CallerMemberName]string propertyName = null)
		//{
		//	ReactiveProperty<T> createReactiveProperty()
		//	{
		//		return new ReactiveProperty<T>(initialValueProvider.Invoke(), mode, equalityComparer);
		//	}

		//	return target.LazyReactiveProperty(createReactiveProperty, propertyName);
		//}

		public static ReactiveProperty<T> LazyReactiveProperty<T>(
			this IReactiveLazyPropertyHolder target,
			IScheduler raiseEventScheduler,
			Func<T> initialValueProvider,
			ReactivePropertyMode mode = ReactivePropertyMode.Default,
			IEqualityComparer<T> equalityComparer = null,
			[CallerMemberName]string propertyName = null)
		{
			ReactiveProperty<T> createReactiveProperty()
			{
				return new ReactiveProperty<T>(initialValueProvider.Invoke(), mode, equalityComparer);
			}

			return target.LazyReactiveProperty(createReactiveProperty, propertyName);
		}

		#endregion with InitialValueProvider

		#region with InitialValue

		public static ReactiveProperty<T> LazyReactiveProperty<T>(
			this IReactiveLazyPropertyHolder target,
			T initialValue,
			ReactivePropertyMode mode = ReactivePropertyMode.Default,
			IEqualityComparer<T> equalityComparer = null,
			[CallerMemberName]string propertyName = null)
		{
			ReactiveProperty<T> createReactiveProperty()
			{
				return new ReactiveProperty<T>(initialValue, mode, equalityComparer);
			}

			return target.LazyReactiveProperty(createReactiveProperty, propertyName);
		}

		public static ReactiveProperty<T> LazyReactiveProperty<T>(
			this IReactiveLazyPropertyHolder target,
			IScheduler raiseEventScheduler,
			T initialValue,
			ReactivePropertyMode mode = ReactivePropertyMode.Default,
			IEqualityComparer<T> equalityComparer = null,
			[CallerMemberName]string propertyName = null)
		{
			ReactiveProperty<T> createReactiveProperty()
			{
				return new ReactiveProperty<T>(raiseEventScheduler, initialValue, mode, equalityComparer);
			}

			return target.LazyReactiveProperty(createReactiveProperty, propertyName);
		}

		#endregion with InitialValue
	}
}
