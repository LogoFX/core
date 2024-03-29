﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace LogoFX.Core.Tests
{
    public class CollectionsExtensionsTests
    {
        [Fact]
        public void ForEachByOne_CollectionIsValid_ActionIsAppliedForEachElement()
        {
            var collection = new[] {"t1", "t2", "t3"};
            var stringBuilder = new StringBuilder();
            Action<string> action = r => stringBuilder.Append(r);
            collection = collection.ForEachByOne(action).ToArray();

            const string expectedResult = "t1t2t3";
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ForEachByOne_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string> action = r => stringBuilder.Append(r);

            var exception = Record.Exception(() => collection.ForEachByOne(action).ToArray());
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void ForEachByOneWithIndex_CollectionIsValid_ActionIsAppliedForEachElementAndIndexIsIncreased()
        {
            var collection = new[] {"t1", "t2", "t3"};
            var stringBuilder = new StringBuilder();
            var indexAggregator = 0;
            Action<string, int> action = (r, i) =>
            {
                stringBuilder.Append(r);
                indexAggregator += i * 3;
            };
            collection = collection.ForEachByOne(action).ToArray();

            const string expectedResult = "t1t2t3";
            const int expectedIndexAggregator = 9;
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);

            actualResult.Should().BeEquivalentTo(expectedResult);
            indexAggregator.Should().Be(expectedIndexAggregator);
        }

        [Fact]
        public void ForEachByOneWithIndex_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string, int> action = (r, t) => stringBuilder.Append(r);

            var exception = Record.Exception(() => collection.ForEachByOne(action).ToArray());
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void ForEachByOneWithRange_CollectionIsValid_ActionIsAppliedForTheElementsWithinRange()
        {
            var collection = new[] {"t1", "t2", "t3"};
            var stringBuilder = new StringBuilder();
            var indexAggregator = 0;
            Action<string, int> action = (r, i) =>
            {
                stringBuilder.Append(r);
                indexAggregator++;
            };
            collection = collection.ForEachByOne(1, 2, action).ToArray();

            const string expectedResult = "t2t3";
            const int expectedAggregator = 2;
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);

            actualResult.Should().BeEquivalentTo(expectedResult);
            indexAggregator.Should().Be(expectedAggregator);
        }

        [Fact]
        public void ForEachByOneWithRange_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string, int> action = (r, t) => stringBuilder.Append(r);

            var exception = Record.Exception(() => collection.ForEachByOne(0, 1, action).ToArray());
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void ForEach_CollectionIsValid_ActionIsAppliedForEachElement()
        {
            var collection = new[] {"t1", "t2", "t3"};
            var stringBuilder = new StringBuilder();
            Action<string> action = r => stringBuilder.Append(r);
            collection = collection.ForEach(action).ToArray();

            const string expectedResult = "t1t2t3";
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);

            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public void ForEach_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string> action = r => stringBuilder.Append(r);

            var exception = Record.Exception(() => collection.ForEach(action).ToArray());
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void ForEachWithIndex_CollectionIsValid_ActionIsAppliedForEachElementAndIndexIsIncreased()
        {
            var collection = new[] {"t1", "t2", "t3"};
            var stringBuilder = new StringBuilder();
            var indexAggregator = 0;
            Action<string, int> action = (r, i) =>
            {
                stringBuilder.Append(r);
                indexAggregator += i * 3;
            };
            collection = collection.ForEach(action).ToArray();

            const string expectedResult = "t1t2t3";
            const int expectedIndexAggregator = 9;
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);

            actualResult.Should().BeEquivalentTo(expectedResult);
            indexAggregator.Should().Be(expectedIndexAggregator);
        }

        [Fact]
        public void ForEachWithIndex_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string, int> action = (r, t) => stringBuilder.Append(r);

            var exception = Record.Exception(() => collection.ForEach(action).ToArray());
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void ForEachWithRange_CollectionIsValid_ActionIsAppliedForTheElementsWithinRange()
        {
            var collection = new[] {"t1", "t2", "t3"};
            var stringBuilder = new StringBuilder();
            var indexAggregator = 0;
            Action<string, int> action = (r, i) =>
            {
                stringBuilder.Append(r);
                indexAggregator++;
            };
            collection = collection.ForEach(1, 2, action).ToArray();

            const string expectedResult = "t2t3";
            const int expectedAggregator = 2;
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);

            actualResult.Should().BeEquivalentTo(expectedResult);
            indexAggregator.Should().Be(expectedAggregator);
        }

        [Fact]
        public void ForEachWithRange_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string, int> action = (r, t) => stringBuilder.Append(r);

            var exception = Record.Exception(() => collection.ForEach(0, 1, action).ToArray());
            exception.Should().BeOfType<ArgumentNullException>();
        }
    }
}
