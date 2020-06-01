using System;
using System.Collections.Generic;
using ChainingAssertion;
using Xunit;
using Xunit.Abstractions;

namespace LazyProperty.Tests
{
	public class PropertyBagExtensionsTests : TestBase
	{
		public PropertyBagExtensionsTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void TestTryGetValueAsObject()
		{
			var bag = new PropertyBag();
			var propertyName = "Property-Name";

			bag.TryGetValueAsObject(propertyName, out _).IsFalse();

			bag.SetValue(propertyName, DateTimeOffset.Now);
			bag.TryGetValueAsObject(propertyName, out _).IsTrue();
		}

		[Fact]
		public void TestSetAndGetValueAsObject()
		{
			var bag = new PropertyBag();
			var propertyName = "Property-Name";

			var values = new[] { 0, 1, 2.2, 3.3f, '4', "five", true, new object(), DateTimeOffset.MaxValue, TimeSpan.Zero };

			Assert.Throws<KeyNotFoundException>(() =>
			{
				bag.GetValueAsObject(propertyName);
			});

			foreach (var value in values)
			{
				bag.SetValueAsObject(propertyName, value);
				bag.GetValueAsObject(propertyName).Is(value);
			}
		}

		[Fact]
		public void TestGetValueOrDefault()
		{
			var bag = new PropertyBag();
			var propertyName = "Property-Name";

			var values = new[] { 0, 1, 2.2, 3.3f, '4', "five", true, new object(), DateTimeOffset.MaxValue, TimeSpan.Zero };

			foreach (var value in values)
			{
				bag.GetValueOrDefault(
					propertyName,
					key =>
					{
						key.Is(propertyName);
						return value;
					})
					.Is(value);

				bag.GetValueOrDefault(propertyName, () => value).Is(value);

				bag.GetValueOrDefault(propertyName, value).Is(value);
			}
		}

		[Fact]
		public void TestDeletePropertyObject()
		{
			var bag = new PropertyBag();
			var propertyName = "Property-Name";

			bag.DeleteProperty(propertyName).IsFalse();
			bag.SetValue(propertyName, 10);
			bag.DeleteProperty(propertyName).IsTrue();
		}
	}
}
