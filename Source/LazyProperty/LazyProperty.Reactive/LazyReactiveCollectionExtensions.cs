using System;
using System.Runtime.CompilerServices;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Studiotaiha.LazyProperty
{
	public static class LazyReactiveCollectionExtensions
	{
		public static ReactiveCollection<T> LazyReactiveCollection<T>(
			this IReactiveLazyPropertyHolder target,
			Func<ReactiveCollection<T>> valueProvider,
			[CallerMemberName]string propertyName = null)
		{
			return target
				.LazyProperty(valueProvider, propertyName)
				.AddTo(target.Disposables);
		}
	}
}
