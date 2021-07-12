using UnityEngine;
using System;
using Assets.Scripts.Types;

namespace Assets.Scripts.Gestures
{
    public class SingleTouchGesture : Gesture
    {
        /// <summary>
        /// The single touch of this single touch gesture.
        /// </summary>
        public Touch Touch { get; }

        protected SingleTouchGesture(Touch touch)
        {
            this.Touch = touch;
        }

        /// <summary>
        /// Generates a SingleTouchGesture from provided touch.
        /// </summary>
        /// <param name="touch">Touch to generate a single touch gesture from.</param>
        /// <returns>The Single Touch Gesture that the provided touch is represented by.</returns>
        public static SingleTouchGesture FromTouch(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    return new StartGesture(touch);

                // TODO: Consider ignoring movement inside a certain threshold (and treat it as stationary instead).
                case TouchPhase.Moved:
                    return new DragGesture(touch);

                case TouchPhase.Stationary:
                    return new HoldGesture(touch);

                case TouchPhase.Ended:
                    return new ReleaseGesture(touch);

                case TouchPhase.Canceled:
                    return new ForcedReleaseGesture(touch);

                default:
                    throw new ArgumentOutOfRangeException($"Touch phase type unrecognized or unimplemented \"{touch.phase.GetType().Name}\".");
            }
        }
    }
}
