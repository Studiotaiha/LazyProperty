using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace LazyProperty
{
	public static class LazyReactiveCommandExtensions
	{
		public static ReactiveCommand LazyReactiveCommand(
			this IReactiveLazyPropertyHolder target,
			Func<ReactiveCommand> valueProvider,
			[CallerMemberName]string propertyName = null)
		{
			return target
				.LazyProperty(valueProvider, propertyName)
				.AddTo(target.Disposables);
		}

		public static ReactiveCommand<T> LazyReactiveCommand<T>(
			this IReactiveLazyPropertyHolder target,
			Func<ReactiveCommand<T>> valueProvider,
			[CallerMemberName]string propertyName = null)
		{
			return target
				.LazyProperty(valueProvider, propertyName)
				.AddTo(target.Disposables);
		}

		public static ReactiveCommand LazyReactiveCommand(
			this IReactiveLazyPropertyHolder target,
			IObservable<bool> executableObservable,
			Action executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			ReactiveCommand createReactiveCommand()
			{
				return executableObservable
					.ToReactiveCommand()
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyReactiveCommand(
				createReactiveCommand,
				propertyName);
		}

		public static ReactiveCommand<T> LazyReactiveCommand<T>(
			this IReactiveLazyPropertyHolder target,
			IObservable<bool> executableObservable,
			Action<T> executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			ReactiveCommand<T> createReactiveCommand()
			{
				return executableObservable
					.ToReactiveCommand<T>()
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyReactiveCommand(
				createReactiveCommand,
				propertyName);
		}

		public static ReactiveCommand LazyReactiveCommand(
			this IReactiveLazyPropertyHolder target,
			bool initialValue,
			Action executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			ReactiveCommand createReactiveCommand()
			{
				return new ReactiveCommand(Observable.Empty<bool>(), initialValue)
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyReactiveCommand(
				createReactiveCommand,
				propertyName);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ReactiveCommand<T> LazyReactiveCommand<T>(
			this IReactiveLazyPropertyHolder target,
			bool initialValue,
			Action<T> executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			ReactiveCommand<T> createReactiveCommand()
			{
				return new ReactiveCommand<T>(Observable.Empty<bool>(), initialValue)
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyReactiveCommand(
				createReactiveCommand,
				propertyName);
		}

		public static ReactiveCommand LazyReactiveCommand(
			this IReactiveLazyPropertyHolder target,
			Action executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			ReactiveCommand createReactiveCommand()
			{
				return new ReactiveCommand()
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyReactiveCommand(
				createReactiveCommand,
				propertyName);
		}

		public static ReactiveCommand<T> LazyReactiveCommand<T>(
			this IReactiveLazyPropertyHolder target,
			Action<T> executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			ReactiveCommand<T> createReactiveCommand()
			{
				return new ReactiveCommand<T>()
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyReactiveCommand(
				createReactiveCommand,
				propertyName);
		}
	}
}
