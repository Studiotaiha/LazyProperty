using System;
using Xunit.Abstractions;

namespace Studiotaiha.LazyProperty.Tests
{
	public class TestBase
	{
		protected ITestOutputHelper Output { get; }

		public TestBase(ITestOutputHelper output)
		{
			this.Output = output ?? throw new ArgumentNullException(nameof(output));
		}
	}
}
