using UnityEngine;

namespace Assets.Scripts.Gestures
{
    public class DragGesture : SingleTouchGesture
    {
        public Vector2 TranslationResult { get; }

        public DragGesture(Touch touch) : base(touch)
        {
            this.TranslationResult = touch.deltaPosition / -25000f;
        }
    }
}
