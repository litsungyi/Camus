namespace Camus.StateMachines
{
    /**
     * A state of StateMachine<TOwner>. Subclass it per state and override the hooks you need.
     */
    public abstract class State<TOwner>
    {
        public StateMachine<TOwner> Machine
        {
            get;
            internal set;
        }

        public TOwner Owner => Machine.Owner;

        protected internal virtual void OnEnter() {}

        protected internal virtual void OnUpdate(float deltaTime) {}

        protected internal virtual void OnExit() {}

        // Shortcut for Machine.ChangeState<TState>()
        protected void ChangeState<TState>() where TState : State<TOwner>
        {
            Machine.ChangeState<TState>();
        }
    }
}
