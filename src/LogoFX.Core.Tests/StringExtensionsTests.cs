using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace LogoFX.Core.Tests
{
    public class StringExtensionsTests
    {
        public static readonly List<object[]> SourceData =
            new List<object[]>
            {
                new object[] {"5", "5"},
                new object[] {"y_s", "ys"},
                new object[] {"yS", "y S"},
                new object[] {"Y s", "Ys"},
                new object[] {"y s", "ys"},
                new object[] {"SomeEnum", "Some Enum"},
            };

        [Theory]
        [MemberData(nameof(SourceData))]
        public void StringBeautify_ProvidingSourceString_ResultIsCorrect(string source, string expectedResult)
        {
            var actualResult = source.Beautify();

            actualResult.Should().Be(expectedResult);
        }
    }
}
