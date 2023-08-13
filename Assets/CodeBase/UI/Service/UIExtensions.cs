using UnityEngine;
using UnityEngine.UI;

namespace UI.Service
{
    public static class UIExtensions
    {
        public static void ChangeImageAlpha(this Image image, float targetAlpha)
        {
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.g, targetAlpha);
        }
    }
}