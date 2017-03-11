using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace GOH.Schedule.Tests
{
    [TestFixture]
    public class ScheduleTests
    {
        const int num_of_items = 10;
        private Item<object>[] items = new Item<object>[num_of_items];
        const int num_of_cycles = 1000;
        private Schedule<object> schedule;
        private Schedule<object> emptySchedule;

        [SetUp]
        public void SetUp()
        {
            // items[0] Period: 0-0
            // items[1] Period: 0-1
            // items[2] Period: 1-3
            // items[3] Period: 3-6
            // items[4] Period: 6-10
            // items[5] Period: 10-15
            // items[6] Period: 15-21
            // items[7] Period: 21-28
            // items[8] Period: 28-36
            // items[9] Period: 36-45
            for (int i = 0; i < num_of_items; i++)
            {
                this.items[i] = new Item<object>((float)i, new object());
            }
            this.schedule = new Schedule<object>(this.items);
            this.emptySchedule = new Schedule<object>();
        }

        // TESTS
        // Behaviour if no items are present
        // Behaviour if only one item is present

        [Test]
        public void Test_CanRunTest()
        {
            System.Console.WriteLine(this.schedule.Items);
        }

        [Test]
        public void Test_ReturnsExpectedLength_OfItems()
        {
            float expectedLength = 0.0f;
            for (int i = 0; i < num_of_items; i++)
            {
                expectedLength += i;
            }
            float actualLength = this.schedule.Length;
            if (actualLength != expectedLength)
            {
                string message = string.Format("Schedule returned different length to expected.\n\tActual  : {0}\n\tExpected: {1}", actualLength, expectedLength);
                Assert.Fail(message);
            }
        }

        [Test]
        public void Test_HasItems_ReturnsFalse_ForEmptySchedule()
        {
            bool expectedHasItems = false;
            bool actual = this.emptySchedule.HasItems;
            checkHasItemsResults(expectedHasItems, actual);
        }

        private static void checkHasItemsResults(bool expectedHasItems, bool actual)
        {
            Assert.AreEqual(expectedHasItems, actual, "hasItems() returned unexpected result");
        }

        [Test]
        public void Test_At_ThrowsException_ForEmptySchedule()
        {
            try
            {
                this.emptySchedule.at(-1.0f);
            }
            catch (System.InvalidOperationException e)
            {
                if (e.Message != Schedule<object>.no_items_string)
                {
                    string message = string.Format("Unexpected error message for empty schedule.\n\tActual  : {0}\n\tExpected: {1}", e.Message, Schedule<object>.no_items_string);
                    Assert.Fail(message);
                }
                Assert.Pass();
            }
            Assert.Fail("Expected exception but none thrown.");
        }

        [Test]
        public void Test_At_ReturnsExpectedResults_ForKnownTimes()
        {
            LinkedList<AtTestSet<object>> testSets = new LinkedList<AtTestSet<object>>();
            testSets.AddLast(new AtTestSet<object>(0f, this.items[1]));
            testSets.AddLast(new AtTestSet<object>(1.0f, this.items[2]));
            testSets.AddLast(new AtTestSet<object>(1.5f, this.items[2]));
            testSets.AddLast(new AtTestSet<object>(2f, this.items[2]));
            testSets.AddLast(new AtTestSet<object>(3f, this.items[3]));
            testSets.AddLast(new AtTestSet<object>(28f, this.items[8]));

            int testIndex = 0;
            string testName = "at";
            foreach (AtTestSet<object> testSet in testSets)
            {
                performAtPeriodTests(testIndex++, testSet);
            }
        }

        private void performAtPeriodTests(int testIndex, AtTestSet<object> testSet)
        {
            string testName = "at";
            testAtKnownTimeInBasePeriod(testName, testSet, testIndex);
            testAtKnownTimeInNegativePeriod(testName, testSet, testIndex);
            testAtKnownTimeInPositiveNonBasePeriod(testName, testSet, testIndex);
        }

        private void testAtKnownTimeInBasePeriod(string testName, AtTestSet<object> testSet, int testIndex)
        {
            this.testAtSet(testName + " base period", testSet, testIndex);
        }

        private void testAtKnownTimeInNegativePeriod(string testName, AtTestSet<object> testSet, int testIndex)
        {
            testSet.Time -= this.schedule.Length;
            this.testAtSet(testName + " negative period", testSet, testIndex);
        }

        private void testAtKnownTimeInPositiveNonBasePeriod(string testName, AtTestSet<object> testSet, int testIndex)
        {
            testSet.Time += 20 * this.schedule.Length;
            this.testAtSet(testName + " positive non-base-period", testSet, testIndex);
        }

        private void testAtSet(string testName, AtTestSet<object> testSet, int testIndex)
        {
            object actual = this.schedule.at(testSet.Time);
            checkResults(testName, testSet, actual, testIndex);
        }

        private void checkResults(string testType, AtTestSet<object> testSet, object actual, int testIndex)
        {
            string message = string.Format("Test index: {0}\nreturned unexpected results for {1} test.\n\tTime: {2}", testIndex, testType, testSet.Time);
            Assert.AreEqual(testSet.ExpectedResult, actual, message);
        }

        [Test]
        public void Test_NextAt_ThrowsException_ForEmptySchedule()
        {
            try
            {
                this.emptySchedule.nextAt(-1.0f);
            }
            catch (System.InvalidOperationException e)
            {
                if (e.Message != Schedule<object>.no_items_string)
                {
                    string message = string.Format("Unexpected error message for empty schedule.\n\tActual  : {0}\n\tExpected: {1}", e.Message, Schedule<object>.no_items_string);
                    Assert.Fail(message);
                }
                Assert.Pass();
            }
            Assert.Fail("Expected exception but none thrown.");
        }

        [Test]
        public void Test_NextAt_ReturnsExpectedResults_ForKnownTimes()
        {
            LinkedList<AtTestSet<object>> testSets = new LinkedList<AtTestSet<object>>();
            testSets.AddLast(new AtTestSet<object>(0f, this.items[2]));
            testSets.AddLast(new AtTestSet<object>(1.0f, this.items[3]));
            testSets.AddLast(new AtTestSet<object>(1.5f, this.items[3]));
            testSets.AddLast(new AtTestSet<object>(2f, this.items[3]));
            testSets.AddLast(new AtTestSet<object>(3f, this.items[4]));
            testSets.AddLast(new AtTestSet<object>(28f, this.items[9]));
            testSets.AddLast(new AtTestSet<object>(44f, this.items[0]));
            testSets.AddLast(new AtTestSet<object>(45f, this.items[2]));

            int testIndex = 0;
            foreach (AtTestSet<object> testSet in testSets)
            {
                performNextAtPeriodTests(testIndex++, testSet);
            }
        }

        private void performNextAtPeriodTests(int testIndex, AtTestSet<object> testSet)
        {
            string testName = "nextAt";
            testNextAtKnownTimeInBasePeriod(testName, testSet, testIndex);
            testNextAtKnownTimeInNegativePeriod(testName, testSet, testIndex);
            testNextAtKnownTimeInPositiveNonBasePeriod(testName, testSet, testIndex);
        }

        private void testNextAtKnownTimeInBasePeriod(string testName, AtTestSet<object> testSet, int testIndex)
        {
            this.testNextAtSet(testName + " base period", testSet, testIndex);
        }

        private void testNextAtKnownTimeInNegativePeriod(string testName, AtTestSet<object> testSet, int testIndex)
        {
            testSet.Time -= this.schedule.Length;
            this.testNextAtSet(testName + " negative period", testSet, testIndex);
        }

        private void testNextAtKnownTimeInPositiveNonBasePeriod(string testName, AtTestSet<object> testSet, int testIndex)
        {
            testSet.Time += 20 * this.schedule.Length;
            this.testNextAtSet(testName + " positive non-base-period", testSet, testIndex);
        }

        private void testNextAtSet(string testName, AtTestSet<object> testSet, int testIndex)
        {
            object actual = this.schedule.nextAt(testSet.Time);
            checkResults(testName, testSet, actual, testIndex);
        }

        [Test]
        public void Test_ScheduleWithSingleItem_ReturnsItem_ForAt_And_NextAt()
        {
            Item<string> item = new Item<string>(39123098f, "NOW THEN");
            Schedule<string> schedule = new Schedule<string>(item);
            Item<string> at = schedule.at(321f);
            Item<string> nextAt = schedule.at(321f);
            Assert.AreSame(item, at, "Single item not returned when at is called.");
            Assert.AreSame(item, nextAt, "Single item not returned when nextAt is called.");
            Assert.AreSame(at, nextAt, "at and nextAt did not return the same item for a schedule with a single item.");
        }

        class AtTestSet<K>
        {
            public float Time { get; set; }
            public Item<K> ExpectedResult { get; set; }

            public AtTestSet(float time, Item<K> expectedResult)
            {
                this.Time = time;
                this.ExpectedResult = expectedResult;
            }
        }
    }
}