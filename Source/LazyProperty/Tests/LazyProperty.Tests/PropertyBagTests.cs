using System;
using System.Collections.Generic;
using ChainingAssertion;
using Xunit;
using Xunit.Abstractions;

namespace LazyProperty.Tests
{
	public class PropertyBagTests : TestBase
	{
		public PropertyBagTests(ITestOutputHelper output) : base(output)
		{
		}

		public static IEnumerable<object[]> GetTestData()
		{
			var testData = new object[]
			{
				true,
				false,

				int.MinValue,
				-1,
				0,
				1,
				int.MaxValue,

				float.NegativeInfinity,
				float.MinValue,
				-1.0f,
				0.0f,
				1.0,
				float.MaxValue,
				float.PositiveInfinity,

				double.NegativeInfinity,
				double.MinValue,
				-1.0,
				0.0,
				1.0,
				double.MaxValue,
				double.PositiveInfinity,

				null,
				"",
				"a",
				"a;dlfja;sodifj;1-64^-7v[4]ghklcxhvo",

				new object(),

				Guid.Empty,
				Guid.NewGuid(),
				DateTimeOffset.Now,
				DateTimeOffset.Now.Offset,
				DateTime.Now,
			};

			foreach (var value in testData)
			{
				//this.Output.WriteLine(value?.ToString() ?? "null");
				yield return new object[] { value };
			}
		}

		[Theory]
		[InlineData(new[] { 0, 0, 1, 2, 2 }, new[] { true, false, true, true, false })]
		public void TestChangeHandlerInvoked<T>(int[] setValues, bool[] shouldInvokedValues)
		{
			var propertyName = "Property-Name";
			var bag = new PropertyBag();

			var prev = default(int);
			for (var i = 0; i < setValues.Length; i++)
			{
				var next = setValues[i];
				var shouldInvoked = shouldInvokedValues[i];

				this.Output.WriteLine("prev: {0}, next: {1}", prev, next);

				var onChangingCalled = false;
				var onChangedCalled = false;

				bag.SetValue(
					propertyName,
					next,
					onChanging: (oldValue, newValue) =>
					{
						oldValue.Is(prev);
						newValue.Is(next);
						onChangingCalled = true;
					},
					onChanged: (oldValue, newValue) =>
					{
						oldValue.Is(prev);
						newValue.Is(next);
						onChangedCalled = true;
					});

				onChangingCalled.Is(shouldInvoked);
				onChangedCalled.Is(shouldInvoked);

				prev = next;
			}
		}

		[Fact]
		public void TestClear()
		{
			var bag = new PropertyBag();

			bag.PropertiesCount.Is(0);

			bag.SetValue("int", 1);
			bag.SetValue("string", 2);

			bag.PropertiesCount.Is(2);

			bag.ClearProperties();
			bag.PropertiesCount.Is(0);
		}

		[Theory]
		[MemberData(nameof(GetTestData))]
		public void TestCurdWithSortOfDataTypes<T>(T value)
		{
			var propertyName = "Property-" + (value?.ToString() ?? "null");

			var bag = new PropertyBag();

			bag.SetValue(propertyName, value).Is(true);
			bag.PropertiesCount.Is(1);

			bag.GetValue<T>(propertyName).Is(value);
			bag.Properties.Is(new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>(propertyName, value),
			});

			bag.TryGetValue(propertyName, out T obtainedValue).Is(true);
			obtainedValue.Is(value);

			bag.DeleteProperty<T>(propertyName).Is(true);
			bag.PropertiesCount.Is(0);
		}

		[Fact]
		public void TestDeleteProperty()
		{
			var bag = new PropertyBag();
			var propertyName = "Property-Name";

			bag.DeleteProperty<object>(propertyName).IsFalse();

			bag.SetValue(propertyName, 10);

			bag.DeleteProperty<object>(propertyName).IsTrue();
		}

		[Fact]
		public void TestTryGetValueReturnFalse()
		{
			var bag = new PropertyBag();

			var propertyName = "Property-Name";

			bag.TryGetValue<int>(propertyName, out _).IsFalse();

			bag.SetValue(propertyName, 1);
			bag.TryGetValue<int>(propertyName, out _).IsTrue();
			bag.TryGetValue<string>(propertyName, out _).IsFalse();
		}
	}
}
