using System;
using System.Collections.Generic;
using System.Linq;
using Camus.Draggables;
using Camus.EventSystems;
using Camus.StateMachines;
using NUnit.Framework;
using Range = Camus.Utilities.Range;

namespace Camus.Tests
{
    public class CamusEditorTests
    {
        private class TestEvent : IEvent {}

        private class TestSource : DragSource {}

        private class TestTarget : DropTarget {}

        private class LogState : State<List<string>>
        {
            protected override void OnEnter() => Owner.Add($"enter {GetType().Name}");
            protected override void OnUpdate(float deltaTime) => Owner.Add($"update {GetType().Name}");
            protected override void OnExit() => Owner.Add($"exit {GetType().Name}");
        }

        private class Idle : LogState {}

        private class Run : LogState {}

        // Moves straight on to Idle from OnEnter, which the machine queues
        private class Bounce : LogState
        {
            protected override void OnEnter()
            {
                base.OnEnter();
                ChangeState<Idle>();
                Owner.Add("bounce done");
            }
        }

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

        [Test]
        public void StateMachine_FollowsTransitionTable()
        {
            var log = new List<string>();
            var machine = new StateMachine<List<string>>(log)
                .AddState(new Idle())
                .AddState(new Run())
                .AddState(new Bounce())
                .AddTransition<Idle, Run>()
                .AddTransition<Run, Bounce>()
                .AddTransition<Bounce, Idle>()
                .AddTransition<Run, Idle>(owner => owner.Contains("stop"));
            var changes = 0;
            machine.StateChanged += (previous, current) => ++changes;

            Assert.Throws<InvalidOperationException>(() => machine.ChangeState<Idle>());
            machine.Start<Idle>();
            machine.Update(0.1f);
            machine.ChangeState<Run>();
            machine.ChangeState<Bounce>();

            Assert.AreEqual(new[]
            {
                "enter Idle",
                "update Idle",
                "exit Idle", "enter Run",
                "exit Run", "enter Bounce", "bounce done",
                "exit Bounce", "enter Idle",
            }, log);
            Assert.IsTrue(machine.IsInState<Idle>());
            Assert.IsInstanceOf<Bounce>(machine.PreviousState);
            Assert.AreEqual(4, changes);

            // Idle -> Bounce is not in the table
            Assert.IsFalse(machine.CanChangeState<Bounce>());
            Assert.Throws<InvalidOperationException>(() => machine.ChangeState<Bounce>());
            Assert.IsTrue(machine.IsInState<Idle>());

            // Run -> Idle is taken by Update once its condition is true
            machine.ChangeState<Run>();
            machine.Update(0.1f);
            Assert.IsTrue(machine.IsInState<Run>());
            log.Add("stop");
            machine.Update(0.1f);
            Assert.IsTrue(machine.IsInState<Idle>());

            Assert.Throws<InvalidOperationException>(() => machine.Start<Run>());
            Assert.Throws<KeyNotFoundException>(() => new StateMachine<List<string>>(log).Start<Idle>());
            Assert.Throws<InvalidOperationException>(() => new StateMachine<List<string>>(log).AddState(machine.GetState<Run>()));
        }
    }
}
