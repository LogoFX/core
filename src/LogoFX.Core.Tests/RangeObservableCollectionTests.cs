using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace LogoFX.Core.Tests
{
    public class RangeObservableCollectionTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RangeObservableCollectionTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Ctor_DoesntThrow()
        {
            var exception = Record.Exception(() => new RangeObservableCollection<int>());

            exception.Should().BeNull();
        }

        [Fact]
        public void Performance_MonitorNumberOfAllocatedThreads()
        {
            int maxNumberOfThreads = 0;

            int currentNumberOfThreads = Process.GetCurrentProcess().Threads.Count;

            _testOutputHelper.WriteLine("Number of threads before run {0}", currentNumberOfThreads);
            for (int j = 0; j < 100; j++)
            {
                var collection = new RangeObservableCollection<int>();
                for (int i = 0; i < 100; i++)
                {
                    collection.Add(i);

                    if (i % 10 == 0)
                    {
                        int tmp = Process.GetCurrentProcess().Threads.Count;
                        if (tmp > maxNumberOfThreads)
                        {
                            maxNumberOfThreads = tmp;
                        }
                    }
                }
            }

            _testOutputHelper.WriteLine("Max number of threads  {0}", maxNumberOfThreads);
            (maxNumberOfThreads - currentNumberOfThreads).Should()
                .BeLessThan(10, "the number of threads should be low");
        }

        [Fact]
        public void Read_CheckLengthAfterAdd_LengthIsUpdated()
        {
            var col = new RangeObservableCollection<int>();
            col.Add(1);

            col.Count.Should().Be(1);
        }

        [Fact]
        public void Read_ExceptionDuringEnumeration_LockReleased()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }

            var col = new RangeObservableCollection<int>(array);

            try
            {
                int x = 0;
                col.ForEach(c =>
                {
                    if (x++ > 50)
                    {
                        throw new Exception();
                    }
                });
            }
            catch (Exception)
            {
                _testOutputHelper.WriteLine("Exception was fired");
            }

            col.Add(3);

            col.Count.Should().Be(101);
        }

        [Fact]
        public void Write_AddElement_ElementAdded()
        {
            var col = new RangeObservableCollection<string>();
            col.Add("a");

            col.First().Should().Be("a");
        }

        [Fact]
        public void Write_AddNull_ElementAdded()
        {
            var col = new RangeObservableCollection<string>();
            var expected = new[] {"a", null};
            col.AddRange(expected);

            col.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Write_AddRange_ElementsAdded()
        {
            var col = new RangeObservableCollection<string>();
            var expected = new[] {"a", "b"};
            col.AddRange(expected);

            col.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Write_ComplexOperation_CollectionUpdatedProperly()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c"});

            col.Add("d");
            col.Remove("b");
            col.Insert(0, "x");
            col.AddRange(new[] {"z", "f", "y"});
            col.RemoveAt(4);
            col.RemoveRange(new[] {"y", "c"});
            col[2] = "p";

            col.Should().BeEquivalentTo("x", "a", "p", "f");
        }

        [Fact]
        public void AddRange_5SequentialAdds_CollectionChangeEventsAreReported()
        {
            var col = new RangeObservableCollection<string>(new[] {"a"});
            var argsList = new List<NotifyCollectionChangedEventArgs>();
            col.CollectionChanged += (sender, args) => { argsList.Add(args); };
            col.AddRange(new[] {"z1", "f1", "y1"});
            col.AddRange(new[] {"z2", "f2", "y2"});
            col.AddRange(new[] {"z3", "f3", "y3"});
            col.AddRange(new[] {"z4", "f4", "y4"});
            col.AddRange(new[] {"z5", "f5", "y5"});

            argsList.Count(x => x.Action == NotifyCollectionChangedAction.Add).Should().Be(5);
            foreach (var args in argsList)
            {
                col.Skip(args.NewStartingIndex).Take(args.NewItems.Count).Should().BeEquivalentTo(args.NewItems.OfType<string>());
            }

            col.Should().BeEquivalentTo("a", "z1", "f1", "y1", "z2", "f2", "y2", "z3", "f3", "y3", "z4", "f4", "y4",
                "z5", "f5", "y5");
        }

        [Fact]
        public void Write_FiresAddEvent()
        {
            var col = new RangeObservableCollection<string>();
            string received = string.Empty;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    received = args.NewItems.OfType<string>().First();
                }
            };
            col.Add("a");

            received.Should().Be("a");
        }

        [Fact]
        public void AddRange_FiresAddEvent()
        {
            var col = new RangeObservableCollection<string>();
            string received = string.Empty;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    received = args.NewItems.OfType<string>().First();
                }
            };
            col.AddRange(new[] {"a", "b", "c"});

            received.Should().Be("a");
        }

        [Fact]
        public void RemoveRange_FiresRemoveEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c"});
            string received = string.Empty;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received = args.OldItems.OfType<string>().First();
                }
            };
            col.RemoveRange(new[] {"a", "b"});

            received.Should().Be("a");
        }

        [Fact]
        public void Write_FiresRemoveEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"b", "c"});
            string received = string.Empty;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received = args.OldItems.OfType<string>().First();
                }
            };
            col.Remove("c");

            received.Should().Be("c");
        }

        [Fact]
        public void Write_FiresResetEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"b", "c"});
            bool fired = false;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    fired = true;
                }
            };
            col.Clear();

            fired.Should().BeTrue();
        }

        [Fact]
        public void Write_InsertElement_ElementInserted()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c"});
            col.Insert(1, "x");

            col.Should().BeEquivalentTo("a", "x", "b", "c");
        }

        [Fact]
        public void Write_InsertElement_FiresAddEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c"});
            NotifyCollectionChangedEventArgs receivedArgs = null;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    receivedArgs = args;
                }
            };
            col.Insert(1, "x");

            receivedArgs.NewStartingIndex.Should().Be(1, "the item should be inserted at the correct position");
            receivedArgs.NewItems.Should().BeEquivalentTo(new[] {"x"}, "the new items collection should be correct");
        }

        [Fact]
        public void Write_RemoveElementAtIndex_ElementRemoved()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c"});
            col.RemoveAt(1);

            col.Should().BeEquivalentTo("a", "c");
        }

        [Fact]
        public void Write_RemoveElement_ElementRemoved()
        {
            var col = new RangeObservableCollection<string>(new[] {"b", "c", "d"});
            col.Remove("b");

            col.Should().BeEquivalentTo("c", "d");
        }

        [Fact]
        public void Write_RemoveNotExisting_DoesntFail()
        {
            var col = new RangeObservableCollection<string>(new[] {"b", "c", "d"});
            col.RemoveRange(new[] {"b", "X"});

            col.Should().BeEquivalentTo("c", "d");
        }

        [Fact]
        public void Write_RemoveRange_ElementsRemoved()
        {
            var col = new RangeObservableCollection<string>(new[] {"b", "c", "d"});
            col.RemoveRange(new[] {"b", "c"});

            col.Should().BeEquivalentTo("d");
        }

        [Fact]
        public void Write_ReplaceElement_ElementReplaced()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c"});
            col[2] = "z";

            col.Should().BeEquivalentTo("a", "b", "z");
        }

        [Fact]
        public void Write_ReplaceElement_FiresReplaceEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c"});
            NotifyCollectionChangedEventArgs receivedArgs = null;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Replace)
                {
                    receivedArgs = args;
                }
            };
            col[2] = "z";

            receivedArgs.NewStartingIndex.Should().Be(2, "the new starting index should be correct");
            receivedArgs.NewItems.Should().BeEquivalentTo(new[] {"z"}, "the new items collection should be correct");
            receivedArgs.OldItems.Should().BeEquivalentTo(new[] {"c"}, "the old items collection should be correct");
        }

        [Fact]
        public void RemoveRange_AcquireRangeToRemoveUsingLinq_RangeRemovedWithoutExceptions()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c"});
            var select = col.Where(c => c.Equals("c"));
            col.RemoveRange(select);

            col.Should().BeEquivalentTo("a", "b");
        }

        [Fact]
        public void RemoveRange_SequentialRemove_StartFromFirstElement_FiresRemoveEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

            var received = new List<string>();
            int oldIndex = -1;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received.AddRange(args.OldItems.OfType<string>());
                    oldIndex = args.OldStartingIndex;
                }
            };
            col.RemoveRange(new[] {"a", "b", "c", "d", "e"});

            oldIndex.Should().Be(0);
            received.Should().BeEquivalentTo("a", "b", "c", "d", "e");
            col.Should().BeEquivalentTo("f", "g");
        }

        [Fact]
        public void RemoveRange_SequentialRemove_StartFromSecondElement_FiresRemoveEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

            var received = new List<string>();
            int oldIndex = -1;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received.AddRange(args.OldItems.OfType<string>());
                    oldIndex = args.OldStartingIndex;
                }
            };
            col.RemoveRange(new[] {"b", "c", "d", "e"});

            oldIndex.Should().Be(1);
            received.Should().BeEquivalentTo("b", "c", "d", "e");
            col.Should().BeEquivalentTo("a", "f", "g");
        }

        [Fact]
        public void RemoveRange_AllRemove_FiresResetEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f"});

            int received = 0;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    ++received;
                }
            };

            col.RemoveRange(new[] {"a", "b", "c", "d", "e", "f"});

            received.Should().Be(1);
            col.Should().BeEmpty();
        }

        [Fact]
        public void RemoveRange_RemoveOneElement_FiresRemoveEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

            var received = new List<string>();
            int oldIndex = -1;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received.AddRange(args.OldItems.OfType<string>());
                    oldIndex = args.OldStartingIndex;
                }
            };
            col.RemoveRange(new[] {"d"});

            oldIndex.Should().Be(3);
            received.Should().BeEquivalentTo("d");
            col.Should().BeEquivalentTo("a", "b", "c", "e", "f", "g");
        }

        [Fact]
        public void RemoveRange_NotSequentialRemove1_FiresRemoveEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

            var received = new List<NotifyCollectionChangedEventArgs>();
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received.Add(args);
                }
            };
            col.RemoveRange(new[] {"b", "c", "a", "d"});

            received.Count.Should().Be(2);
            received[0].OldItems.Should().BeEquivalentTo(new[] {"b", "c"});
            received[0].OldStartingIndex.Should().Be(1);
            received[1].OldItems.Should().BeEquivalentTo(new[] {"a", "d"});
            received[1].OldStartingIndex.Should().Be(0);
            col.Should().BeEquivalentTo("e", "f", "g");
        }

        [Fact]
        public void RemoveRange_NotSequentialRemove2_FiresRemoveEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

            var received = new List<NotifyCollectionChangedEventArgs>();
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received.Add(args);
                }
            };
            col.RemoveRange(new[] {"b", "c", "f", "g"});

            received.Count.Should().Be(2);
            received[0].OldItems.Should().BeEquivalentTo(new[] {"b", "c"});
            received[0].OldStartingIndex.Should().Be(1);
            received[1].OldItems.Should().BeEquivalentTo(new[] {"f", "g"});
            received[1].OldStartingIndex.Should().Be(3);
            col.Should().BeEquivalentTo("a", "d", "e");
        }

        [Fact]
        public void RemoveRange_NotSequentialRemove3_FiresRemoveEvent()
        {
            var col = new RangeObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

            var received = new List<NotifyCollectionChangedEventArgs>();
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received.Add(args);
                }
            };
            col.RemoveRange(new[] {"b", "c", "a", "g"});

            received.Count.Should().Be(3);
            received[0].OldItems.Should().BeEquivalentTo(new[] {"b", "c"});
            received[0].OldStartingIndex.Should().Be(1);
            received[1].OldItems.Should().BeEquivalentTo(new[] {"a"});
            received[1].OldStartingIndex.Should().Be(0);
            received[2].OldItems.Should().BeEquivalentTo(new[] {"g"});
            received[2].OldStartingIndex.Should().Be(3);
            col.Should().BeEquivalentTo("d", "e", "f");
        }
    }
}
