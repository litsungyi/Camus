namespace Camus.Draggables
{
    public enum DraggableResult
    {
        Denied,
        Accepted,
    }

    public enum DroppableResult
    {
        Denied,
        Accepted,
    }

    public enum MergeResult
    {
        // Target took the whole source stack, source is destroyed
        Stacked,
        // Target is filled to max stack, source keeps the rest and moves back to origin
        Overflowed,
        // Source and target combined into target, source is destroyed
        Combined,
        // Source moves into target, target moves to source's origin
        Swapped,
    }
}
