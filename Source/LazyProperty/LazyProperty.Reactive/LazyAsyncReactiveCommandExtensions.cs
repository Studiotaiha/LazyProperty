using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Studiotaiha.LazyProperty
{
	public static class LazyAsyncReactiveCommandExtensions
	{
		public static AsyncReactiveCommand LazyAsyncReactiveCommand(
			this IReactiveLazyPropertyHolder target,
			Func<AsyncReactiveCommand> valueProvider,
			[CallerMemberName]string propertyName = null)
		{
			return target
				.LazyProperty(valueProvider, propertyName)
				.AddTo(target.Disposables);
		}

		public static AsyncReactiveCommand<T> LazyAsyncReactiveCommand<T>(
			this IReactiveLazyPropertyHolder target,
			Func<AsyncReactiveCommand<T>> valueProvider,
			[CallerMemberName]string propertyName = null)
		{
			return target
				.LazyProperty(valueProvider, propertyName)
				.AddTo(target.Disposables);
		}

		public static AsyncReactiveCommand LazyAsyncReactiveCommand(
			this IReactiveLazyPropertyHolder target,
			IObservable<bool> executableObservable,
			Func<Task> executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			AsyncReactiveCommand createAsyncReactiveCommand()
			{
				return executableObservable
					.ToAsyncReactiveCommand()
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyAsyncReactiveCommand(
				createAsyncReactiveCommand,
				propertyName);
		}

		public static AsyncReactiveCommand<T> LazyAsyncReactiveCommand<T>(
			this IReactiveLazyPropertyHolder target,
			IObservable<bool> executableObservable,
			Func<T, Task> executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			AsyncReactiveCommand<T> createAsyncReactiveCommand()
			{
				return executableObservable
					.ToAsyncReactiveCommand<T>()
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyAsyncReactiveCommand(
				createAsyncReactiveCommand,
				propertyName);
		}

		public static AsyncReactiveCommand LazyAsyncReactiveCommand(
			this IReactiveLazyPropertyHolder target,
			bool initialValue,
			Func<Task> executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			AsyncReactiveCommand createAsyncReactiveCommand()
			{
				return new AsyncReactiveCommand(Observable.Empty(initialValue))
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyAsyncReactiveCommand(
				createAsyncReactiveCommand,
				propertyName);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AsyncReactiveCommand<T> LazyAsyncReactiveCommand<T>(
			this IReactiveLazyPropertyHolder target,
			bool initialValue,
			Func<T, Task> executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			AsyncReactiveCommand<T> createAsyncReactiveCommand()
			{
				return new AsyncReactiveCommand<T>(Observable.Empty(initialValue))
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyAsyncReactiveCommand(
				createAsyncReactiveCommand,
				propertyName);
		}

		public static AsyncReactiveCommand LazyAsyncReactiveCommand(
			this IReactiveLazyPropertyHolder target,
			Func<Task> executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			AsyncReactiveCommand createAsyncReactiveCommand()
			{
				return new AsyncReactiveCommand()
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyAsyncReactiveCommand(
				createAsyncReactiveCommand,
				propertyName);
		}

		public static AsyncReactiveCommand<T> LazyAsyncReactiveCommand<T>(
			this IReactiveLazyPropertyHolder target,
			Func<T, Task> executeHandler,
			[CallerMemberName]string propertyName = null)
		{
			AsyncReactiveCommand<T> createAsyncReactiveCommand()
			{
				return new AsyncReactiveCommand<T>()
					.WithSubscribe(executeHandler, x => x.AddTo(target.Disposables));
			}

			return target.LazyAsyncReactiveCommand(
				createAsyncReactiveCommand,
				propertyName);
		}
	}
}
