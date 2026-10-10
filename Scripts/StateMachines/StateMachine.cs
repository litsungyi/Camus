using System;
using System.Collections.Generic;

namespace Camus.StateMachines
{
    /**
     * A finite state machine with one instance per state type and an explicit transition table.
     *
     *   machine.AddState(new Menu()).AddState(new Play())
     *       .AddTransition<Menu, Play>()                          // allowed, changed by ChangeState<Play>()
     *       .AddTransition<Play, Menu>(owner => owner.IsOver);    // also taken by Update when the condition is true
     *   machine.Start<Menu>();
     *
     * The owner drives it by calling Update(deltaTime), e.g. from MonoBehaviour.Update or IUpdatable.OnUpdate.
     * ChangeState called from OnExit/OnEnter is queued and runs after the current change finishes.
     */
    public sealed class StateMachine<TOwner>
    {
        private class Transition
        {
            public State<TOwner> To;
            public Func<TOwner, bool> Condition;
        }

        private readonly Dictionary<Type, State<TOwner>> states = new Dictionary<Type, State<TOwner>>();
        private readonly Dictionary<State<TOwner>, List<Transition>> transitions = new Dictionary<State<TOwner>, List<Transition>>();
        private readonly Queue<State<TOwner>> pendingStates = new Queue<State<TOwner>>();
        private bool isChanging;

        public StateMachine(TOwner owner)
        {
            Owner = owner;
        }

        public TOwner Owner
        {
            get;
        }

        public State<TOwner> CurrentState
        {
            get;
            private set;
        }

        public State<TOwner> PreviousState
        {
            get;
            private set;
        }

        // (previous, current), raised after the new state's OnEnter
        public event Action<State<TOwner>, State<TOwner>> StateChanged;

        #region Definition

        public StateMachine<TOwner> AddState(State<TOwner> state)
        {
            if (state.Machine != null)
            {
                throw new InvalidOperationException($"{state.GetType().Name} already belongs to a state machine");
            }

            states.Add(state.GetType(), state);
            state.Machine = this;
            return this;
        }

        /**
         * Allow changing from TFrom to TTo. With a condition, Update also changes state when it returns true.
         * Conditions are checked in the order they were added. TFrom == TTo allows re-entering a state.
         */
        public StateMachine<TOwner> AddTransition<TFrom, TTo>(Func<TOwner, bool> condition = null)
            where TFrom : State<TOwner>
            where TTo : State<TOwner>
        {
            var from = GetRequiredState(typeof(TFrom));
            if (!transitions.TryGetValue(from, out var list))
            {
                list = new List<Transition>();
                transitions.Add(from, list);
            }

            list.Add(new Transition { To = GetRequiredState(typeof(TTo)), Condition = condition });
            return this;
        }

        public bool HasState<TState>() where TState : State<TOwner>
        {
            return states.ContainsKey(typeof(TState));
        }

        public TState GetState<TState>() where TState : State<TOwner>
        {
            return states.TryGetValue(typeof(TState), out var state) ? (TState) state : null;
        }

        #endregion

        #region Runtime

        public bool IsRunning => CurrentState != null;

        public bool IsInState<TState>() where TState : State<TOwner>
        {
            return CurrentState is TState;
        }

        public bool CanChangeState<TState>() where TState : State<TOwner>
        {
            return CurrentState != null &&
                   states.TryGetValue(typeof(TState), out var to) &&
                   HasTransition(CurrentState, to);
        }

        /**
         * Enter the initial state. Call Stop() first to start again.
         */
        public void Start<TState>() where TState : State<TOwner>
        {
            if (IsRunning || isChanging)
            {
                throw new InvalidOperationException("The state machine is already running");
            }

            Request(GetRequiredState(typeof(TState)));
        }

        /**
         * Exit the current state and enter TState. Throws if the transition from the current state is not defined.
         */
        public void ChangeState<TState>() where TState : State<TOwner>
        {
            if (!IsRunning && !isChanging)
            {
                throw new InvalidOperationException("Start the state machine before changing state");
            }

            Request(GetRequiredState(typeof(TState)));
        }

        public void Update(float deltaTime)
        {
            if (CurrentState == null)
            {
                return;
            }

            CurrentState.OnUpdate(deltaTime);
            if (CurrentState == null || !transitions.TryGetValue(CurrentState, out var list))
            {
                return;
            }

            foreach (var transition in list)
            {
                if (transition.Condition != null && transition.Condition(Owner))
                {
                    Request(transition.To);
                    return;
                }
            }
        }

        /**
         * Exit the current state without entering another, e.g. when the owner is destroyed.
         */
        public void Stop()
        {
            if (CurrentState == null)
            {
                return;
            }

            CurrentState.OnExit();
            PreviousState = CurrentState;
            CurrentState = null;
        }

        #endregion

        private void Request(State<TOwner> next)
        {
            pendingStates.Enqueue(next);
            if (isChanging)
            {
                return;
            }

            isChanging = true;
            try
            {
                while (pendingStates.Count > 0)
                {
                    Change(pendingStates.Dequeue());
                }
            }
            finally
            {
                isChanging = false;
                pendingStates.Clear();
            }
        }

        private void Change(State<TOwner> next)
        {
            var previous = CurrentState;
            // Start has no current state, every later change must be in the table
            if (previous != null && !HasTransition(previous, next))
            {
                throw new InvalidOperationException($"No transition from {previous.GetType().Name} to {next.GetType().Name}");
            }

            previous?.OnExit();

            PreviousState = previous;
            CurrentState = next;
            next.OnEnter();

            StateChanged?.Invoke(previous, next);
        }

        private bool HasTransition(State<TOwner> from, State<TOwner> to)
        {
            return transitions.TryGetValue(from, out var list) && list.Exists(transition => transition.To == to);
        }

        private State<TOwner> GetRequiredState(Type type)
        {
            if (!states.TryGetValue(type, out var state))
            {
                throw new KeyNotFoundException($"{type.Name} is not added to the state machine");
            }

            return state;
        }
    }
}
