# Drag and drop

`Scripts/Draggables`

- Built on uGUI `EventSystem` handlers. Subclass `DragSource` (begin/drag/end) and `DropTarget` (drop). The scene needs an `EventSystem`, and objects need raycast targets.
- `DragManager` (static) tracks active drags and moves the source to the pointer position (screen space, which suits a screen-space canvas). It records the source's origin (parent, sibling index, position) when the drag begins.
- Drop rules:
  - A drop that doesn't land on a `DropTarget`, or lands on one that is not `IsDroppable` or whose `GetDroppableResult` is Denied, fails: the source moves back to its origin and gets `OnCancelDragging`.
  - On an empty target (or its own target), the source moves to `DropTarget.DropPoint` (the serialized `dropPoint`, or the target's transform). The framework tracks `DropTarget.Item` and `DragSource.Target`.
  - On an occupied target, `DragSource.Merge(targetItem)` updates both items' data and returns a `MergeResult`. `Stacked` / `Combined` destroy the source. `Overflowed` moves the source back to origin. `Swapped` (the default) moves the source in, and moves the target's item to the source's old target or origin.
- Override hooks:
  - `DragSource.GetDraggableResult()`: Accepted or Denied.
  - `DragSource.Merge(target)`: a `MergeResult`. Example 3 (`SourceItem`) implements max stack, combine recipes, and swap.
  - `DropTarget.GetDroppableResult(source)`: Accepted or Denied.
  - Callbacks: `OnBeginDragging`, `OnEndDragging` (always last), `OnCancelDragging` (failed drop), `OnDropped`.
- Global events: `BeginDragEvent`, `EndDragEvent`, and `DropEvent(source, target)`.
- **Caution:** `DropTarget.IsDroppable` defaults to `false`. Set it to `true`, or drops are ignored.
