using Camus.EventSystems;

namespace Camus.Draggables.Events
{
    public class EndDragEvent : IDomainEvent
    {
        public EndDragEvent(DragSource source)
        {
            Source = source;
        }

        public DragSource Source
        {
            get;
            private set;
        }
    }
}
