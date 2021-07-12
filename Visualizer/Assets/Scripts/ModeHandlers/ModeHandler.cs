using UnityEngine;
using Assets.Scripts.Gestures;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.ModeHandlers
{
    public abstract class ModeHandler
    {
        /// <summary>
        /// Tells this handler to handle the gesture.
        /// </summary>
        /// <param name="gesture">Gesture to handle by this handler.</param>
        public void Handle(Gesture gesture)
        {
            if (gesture is ZoomGesture zoom)
                this.HandleZoom(zoom);

            else if (gesture is RotateGesture rotate)
                this.HandleRotate(rotate);

            else if (gesture is DragGesture drag)
                this.HandleDrag(drag);

            else if (gesture is HoldGesture hold)
                this.HandleHold(hold);

            else if (gesture is ReleaseGesture release)
                this.HandleRelease(release);

            else if (gesture is StartGesture start)
                this.HandleStart(start);

            else Debug.LogWarning($"ModeHandler doesn't implement {gesture.GetType().Name.Truncate("Gesture".Length)} gesture type.");
        }

        protected virtual void HandleZoom(ZoomGesture gesture) { }

        protected virtual void HandleRotate(RotateGesture gesture) { }

        protected virtual void HandleDrag(DragGesture gesture) { }

        protected virtual void HandleHold(HoldGesture gesture) { }

        protected virtual void HandleRelease(ReleaseGesture gesture) { }

        protected virtual void HandleStart(StartGesture start) { }
    }
}
