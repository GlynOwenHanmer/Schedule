using System;
using NUnit.Framework;
using UnityEngine;
using GOH.Schedule.Tests;
using GOH.Schedule;
using System.Collections.Generic;

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
            for (int i = 0; i < num_of_items; i++)
            {
                this.items[i] = new Item<object>((float)i, new object());
            }
            this.schedule = new Schedule<object>(this.items);
            this.emptySchedule = new Schedule<object>();
        }

        // TESTS
        // Behaviour if no items are present
        // Behaviour when time wraps around
        // Bevaviour when time is exactly on a crossover

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

            int testIndex = 0;
            foreach (AtTestSet<object> testSet in testSets)
            {
                object actual = this.schedule.at(testSet.Time);
                checkAtResults(actual, testSet.ExpectedResult, testIndex++);
            }
        }

        private void checkAtResults(object actual, object expected)
        {
            Assert.AreEqual(actual, expected, "at() returned unexpected results.");
        }

        private void checkAtResults(object actual, object expected, int testIndex)
        {
            string message = string.Format("Test index: {0}\nat() returned unexpected results.", testIndex);
            Assert.AreEqual(actual, expected, message);
        }

        [Test]
        public void Test_NextAt_ThrowsException_ForEmptySchedule()
        {
            Assert.Fail();
        }

        [Test]
        public void Test_NextAt_ThrowsException_ForNullTime()
        {
            Assert.Fail();
        }

        [Test]
        public void Test_NextAt_ReturnsExpectedResults_ForKnownTimes()
        {
            Assert.Fail();
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