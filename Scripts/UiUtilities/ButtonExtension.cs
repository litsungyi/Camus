using UnityEngine;
using UnityEngine.UI;

namespace Camus.UiUtilities
{
    public static class ButtonExtension
    {
        public static void SetColor(this Button button, Color color)
        {
            button.image.color = color;
        }
    }
}
