using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Camus.Localizables;

namespace Camus.Drawers
{
    [CustomPropertyDrawer(typeof(LocalKey))]
    public class LocalKeyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var keyField = new PropertyField(property.FindPropertyRelative("localKey"));
            container.Add(keyField);
            return container;
        }
    }
}
