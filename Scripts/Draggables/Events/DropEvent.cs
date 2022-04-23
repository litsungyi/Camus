using Camus.EventSystems;

namespace Camus.Draggables.Events
{
    public class DropEvent : IDomainEvent
    {
        public DropEvent(DragSource source, DropTarget target)
        {
            Source = source;
            Target = target;
        }

        public DragSource Source
        {
            get;
            private set;
        }

        public DropTarget Target
        {
            get;
            private set;
        }
    }
}
