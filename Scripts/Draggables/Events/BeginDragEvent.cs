using Camus.EventSystems;

namespace Camus.Draggables.Events
{
    public class BeginDragEvent : IDomainEvent
    {
        public BeginDragEvent(DragSource source)
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
