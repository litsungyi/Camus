using System;
using System.Linq;
using Camus.EventSystems;
using NUnit.Framework;
using Range = Camus.Utilities.Range;

namespace Camus.Tests
{
    public class CamusEditorTests
    {
        private class TestEvent : IEvent {}

        [Test]
        public void Range_FactoriesRespectBounds()
        {
            Assert.AreEqual(new[] { 1, 2, 3 }, Range.CreateCloseRange(1, 3).ToArray());
            Assert.AreEqual(new[] { 2 }, Range.CreateOpenRange(1, 3).ToArray());
            Assert.AreEqual(new[] { 2, 3 }, Range.CreateOpenCloseRange(1, 3).ToArray());
            Assert.AreEqual(new[] { 1, 2 }, Range.CreateCloseOpenRange(1, 3).ToArray());
            Assert.IsEmpty(Range.CreateCloseOpenRange(3, 3).ToArray());
            Assert.Throws<ArgumentException>(() => new Range(5, 1));
        }

        [Test]
        public void EventSourcing_InstancesAreIsolated()
        {
            var first = new EventSourcing();
            var second = new EventSourcing();
            var firstCount = 0;
            var secondCount = 0;
            Action<TestEvent> handler = _ => ++firstCount;
            first.Register(handler);
            second.Register<TestEvent>(_ => ++secondCount);

            first.Raise(new TestEvent());
            first.Unregister(handler);
            first.Raise(new TestEvent());

            Assert.AreEqual(1, firstCount);
            Assert.AreEqual(0, secondCount);
        }
    }
}
