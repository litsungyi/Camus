using System;
using System.Linq;
using Camus.Draggables;
using Camus.EventSystems;
using NUnit.Framework;
using Range = Camus.Utilities.Range;

namespace Camus.Tests
{
    public class CamusEditorTests
    {
        private class TestEvent : IEvent {}

        private class TestSource : DragSource {}

        private class TestTarget : DropTarget {}

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

        [Test]
        public void DragManager_SwapsOnOccupiedTargetAndReturnsFailedDrops()
        {
            var root = new UnityEngine.GameObject("root").transform;
            try
            {
                T Create<T>() where T : UnityEngine.Component
                {
                    var component = new UnityEngine.GameObject().AddComponent<T>();
                    component.transform.SetParent(root);
                    return component;
                }

                // Like a real drag, move the source away from its origin before dropping
                void DragAndDrop(DragSource source, DropTarget target)
                {
                    DragManager.BeginDrag(source, null);
                    source.transform.SetParent(root);
                    DragManager.Drop(source.gameObject, target);
                    DragManager.EndDrag(source, null);
                }

                var slotA = Create<TestTarget>();
                var slotB = Create<TestTarget>();
                slotA.IsDroppable = true;
                slotB.IsDroppable = true;
                var first = Create<TestSource>();
                var second = Create<TestSource>();

                DragAndDrop(first, slotA);
                DragAndDrop(second, slotB);
                DragAndDrop(first, slotB);

                Assert.AreSame(first, slotB.Item);
                Assert.AreSame(second, slotA.Item);
                Assert.AreSame(slotA, second.Target);
                Assert.AreSame(slotA.DropPoint, second.transform.parent);

                slotA.IsDroppable = false;
                DragAndDrop(first, slotA);

                Assert.AreSame(first, slotB.Item);
                Assert.AreSame(slotB.DropPoint, first.transform.parent);
                Assert.IsFalse(first.IsDragging);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root.gameObject);
            }
        }
    }
}
