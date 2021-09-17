using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace LogoFX.Core.Tests
{
    public class ConcurrentObservableCollectionTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ConcurrentObservableCollectionTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CopyTo_CopyCollectionWhileRemoving()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }

            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() =>
            {
                for (int i = 100; i >= 0; i--)
                {
                    col.Remove(i);
                    Task.Delay(1).Wait();
                }
            });
            var copy = new int[100];

            var task2 = new Task(() => col.CopyTo(copy, 0));
            task1.Start();
            Task.Delay(10).Wait();
            task2.Start();
            Task.WaitAll(task1, task2);

            copy[2].Should().Be(2);
        }

        [Fact]
        public void Ctor_DoesntThrow()
        {
            var exception = Record.Exception(() => new ConcurrentObservableCollection<int>());

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
                var threadSafe = new ConcurrentObservableCollection<int>();
                for (int i = 0; i < 100; i++)
                {
                    threadSafe.Add(i);

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
        public void ReadWrite_EnumeratingThreadTriesToWriteToCollection_NoDeadlock()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }

            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() => col.ForEach(col.Add));
            task1.Start();

            Task.WaitAll(task1);

            col.Count.Should().Be(200);
        }

        [Fact]
        public void Read_2ConcurrentEnumerations_BothEnumerationsAreExecutedAtSameTime()
        {
            var array = new int[100];

            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }

            var col = new ConcurrentObservableCollection<int>(array);

            int res1 = 1;
            var task1 = new Task(() => col.ForEach(c =>
            {
                Task.Delay(1).Wait();
                res1 = 1;
            }));
            var task2 = new Task(() => col.ForEach(c =>
            {
                Task.Delay(1).Wait();
                res1++;
            }));
            task1.Start();
            task2.Start();
            Task.WaitAll(task1, task2);

            res1.Should().BeLessThan(99);
        }

        [Fact]
        public void Read_CheckContainsWhileAdding()
        {
            var col = new ConcurrentObservableCollection<int>(new[] {1, 2, 3, 4, 5});
            var task1 = new Task(() =>
            {
                for (int i = 10; i < 1000; i++)
                {
                    col.Add(i);
                    Thread.Sleep(1);
                }
            });
            bool contains = false;
            var task2 = new Task(() => { contains = col.Contains(1); });
            task1.Start();
            Thread.Sleep(5);
            task2.Start();
            Task.WaitAll(task1, task2);

            contains.Should().BeTrue();
        }

        [Fact]
        public void Read_CheckLengthAfterAdd_LengthIsUpdated()
        {
            var col = new ConcurrentObservableCollection<int>();
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

            var col = new ConcurrentObservableCollection<int>(array);

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
        public void ToString_ProperlyPrinted()
        {
            var col = new ConcurrentObservableCollection<int>(new[] {1, 2, 3});

            col.ToString().Should().Be("1, 2, 3");
        }

        [Fact]
        public void WriteAndRead_ConcurrentReadAndWrite_SuccessfullyWritesElementsToCollection()
        {
            var array = new int[50];

            for (int i = 0; i < 50; i++)
            {
                array[i] = i;
            }

            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() =>
            {
                for (int i = 50; i < 100; i++)
                {
                    col.Add(i);
                }
            });
            int current = 0;
            var task2 = new Task(() => col.ForEach(c => { current = c; }));
            task1.Start();
            task2.Start();
            Task.WaitAll(task1, task2);

            col.Count.Should().Be(100, "the collection should be populated properly");
            current.Should().BeInRange(49, 100, " the enumeration should have run in-sync with the update");
        }

        [Fact]
        public void Write_AddElement_ElementAdded()
        {
            var col = new ConcurrentObservableCollection<string>();
            col.Add("a");

            col.First().Should().Be("a");
        }

        [Fact]
        public void Write_AddNull_ElementAdded()
        {
            var col = new ConcurrentObservableCollection<string>();
            var expected = new[] {"a", null};
            col.AddRange(expected);

            col.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Write_AddRange_ElementsAdded()
        {
            var col = new ConcurrentObservableCollection<string>();
            var expected = new[] {"a", "b"};
            col.AddRange(expected);

            col.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Write_ComplexOperation_CollectionUpdatedProperly()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c"});
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
            var col = new ConcurrentObservableCollection<string>(new[] {"a"});
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
        public void Write_ComplexUpdateOperationFrom2ThreadsAndEnumerationInTheMiddle_CollectionUpdatedProperly()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }

            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() =>
            {
                for (int i = 100; i < 200; i++)
                {
                    _testOutputHelper.WriteLine("Add {0}", i);
                    col.Add(i);
                }
            });
            var task2 = new Task(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    _testOutputHelper.WriteLine("Remove {0}", i);
                    col.Remove(i);
                }
            });

            var list = new List<int>();
            var task3 = new Task(() => col.ForEach(c =>
            {
                _testOutputHelper.WriteLine("Enumerating {0}", c);
                list.Add(c);
            }));
            task1.Start();
            task2.Start();
            task3.Start();

            Task.WaitAll(task1, task2, task3);

            var expected = new int[100];
            for (int i = 100; i < 200; i++)
            {
                expected[i - 100] = i;
            }

            col.Should().BeEquivalentTo(expected, "the collection should be updated properly");
            list.Should().NotBeEmpty("the enumeration should find at least one element");
        }

        [Fact]
        public void Write_ComplexUpdateOperationFrom2Threads_CollectionUpdatedProperly()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }

            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() =>
            {
                for (int i = 100; i < 200; i++)
                {
                    col.Add(i);
                }
            });
            var task2 = new Task(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    col.Remove(i);
                }
            });
            task1.Start();
            task2.Start();
            Task.WaitAll(task1, task2);

            var expected = new int[100];
            for (int i = 100; i < 200; i++)
            {
                expected[i - 100] = i;
            }

            col.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Write_ConcurrentWrite_SuccessfullyWritesElementsToCollection()
        {
            var col = new ConcurrentObservableCollection<int>();

            var task1 = new Task(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    col.Add(i);
                    Thread.Sleep(1);
                }
            });
            var task2 = new Task(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    col.Clear();
                    Thread.Sleep(1);
                }
            });
            task1.Start();
            task2.Start();
            Task.WaitAll(task1, task2);

            col.Count.Should().BeLessThan(1000);
        }

        [Fact]
        public void Write_FiresAddEvent()
        {
            var col = new ConcurrentObservableCollection<string>();
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
            var col = new ConcurrentObservableCollection<string>();
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
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c"});
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
            var col = new ConcurrentObservableCollection<string>(new[] {"b", "c"});
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
            var col = new ConcurrentObservableCollection<string>(new[] {"b", "c"});
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
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c"});
            col.Insert(1, "x");

            col.Should().BeEquivalentTo("a", "x", "b", "c");
        }

        [Fact]
        public void Write_InsertElement_FiresAddEvent()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c"});
            NotifyCollectionChangedEventArgs receivedArgs = null;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    receivedArgs = args;
                }
            };
            col.Insert(1, "x");

            receivedArgs.NewStartingIndex.Should().Be(1, "the index should be correct");
            receivedArgs.NewItems.Should().BeEquivalentTo(new[] {"x"});
        }

        [Fact]
        public void Write_RemoveElementAtIndex_ElementRemoved()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c"});
            col.RemoveAt(1);

            col.Should().BeEquivalentTo("a", "c");
        }

        [Fact]
        public void Write_RemoveElement_ElementRemoved()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"b", "c", "d"});
            col.Remove("b");

            col.Should().BeEquivalentTo("c", "d");
        }

        [Fact]
        public void Write_RemoveNotExisting_DoesntFail()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"b", "c", "d"});

            col.RemoveRange(new[] {"b", "X"});

            col.Should().BeEquivalentTo("c", "d");
        }

        [Fact]
        public void Write_RemoveRange_ElementsRemoved()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"b", "c", "d"});

            col.RemoveRange(new[] {"b", "c"});

            col.Should().BeEquivalentTo("d");
        }

        [Fact]
        public void Write_ReplaceElement_ElementReplaced()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c"});
            col[2] = "z";

            col.Should().BeEquivalentTo("a", "b", "z");
        }

        [Fact]
        public void Write_ReplaceElement_FiresReplaceEvent()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c"});
            NotifyCollectionChangedEventArgs receivedArgs = null;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Replace)
                {
                    receivedArgs = args;
                }
            };
            col[2] = "z";

            receivedArgs.NewStartingIndex.Should().Be(2);
            receivedArgs.NewItems.Should().BeEquivalentTo(new[] {"z"});
            receivedArgs.OldItems.Should().BeEquivalentTo(new[] {"c"});
        }

        [Fact]
        public void RemoveRange_AcquireRangeToRemoveUsingLinq_RangeRemovedWithoutExceptions()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c"});
            var select = col.Where(c => c.Equals("c"));
            col.RemoveRange(select);

            col.Should().BeEquivalentTo("a", "b");
        }

        [Fact]
        public void RemoveRange_SequentialRemove_StartFromFirstElement_FiresRemoveEvent()
        {
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

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
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

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
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f"});

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
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});
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
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

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
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

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
            var col = new ConcurrentObservableCollection<string>(new[] {"a", "b", "c", "d", "e", "f", "g"});

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