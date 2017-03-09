using NUnit.Framework;
using System;

namespace GOH.Schedule.Tests
{
    [TestFixture]
    public class ItemTests
    {
        [Test]
        public void Test_ScheduleItem_CanBeCreated()
        {
            Item<object> item = new Item<object>(1.0f, new object());
        }

        [Test]
        public void Cant_CreateNegativeLengthItem()
        {
            try
            {
                Item<object> item = new Item<object>(-0.01f, new object());
            }
            catch (ArgumentException e)
            {
                if (e.Message != Item<object>.negative_length_error)
                {
                    string message = string.Format("Wrong error message given for negative length item.\n\tMessage: {0}", e.Message);
                    Assert.Fail(message);
                }
                Assert.Pass("ArgumentException thrown correctly for negative length item.");
                return;
            }
            catch (Exception)
            {
                throw;
            }
            Assert.Fail("Creating item with negative length did not throw expection.");
        }

        [Test]
        public void Item_ReturnsExpectedContent()
        {
            object expected = new object();
            object dummy = new object();
            Item<object> item = new Item<object>(500.0f, expected);
            object actual = item.Content;
            if (actual == dummy)
            {
                string message = string.Format("Item returned unexpected content.\n\tReturned: {0}", dummy);
                Assert.Fail(message);
            }
            if (actual != expected)
            {
                string message = string.Format("Item did not return expected content\n\tActual: {0}\n\tExpected: {1}", actual, expected);
                Assert.Fail(message);
            }
        }
    }
}