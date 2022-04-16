using Camus.Projects;
using UnityEngine;

namespace Camus.Utilities
{
    public static class ComponentExtends
    {
        public static bool IsTagged(this Component component, TagInfo tagInfo) => component != null && component.CompareTag(tagInfo.Name);
    }
}
