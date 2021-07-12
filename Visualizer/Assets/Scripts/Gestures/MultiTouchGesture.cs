using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Linq;

namespace Assets.Scripts.Gestures
{
    public class MultiTouchGesture : Gesture
    {
        protected Touch[] Touches { get; }

        /// <summary>
        /// The amount of touches in this multi-touch gesture.
        /// </summary>
        public int TouchCount => this.Touches.Length;

        /// <summary>
        /// Retrieve the touch at index i.
        /// </summary>
        /// <param name="i">The index at which to retrieve a touch.</param>
        /// <returns>The touch indexed at i.</returns>
        public Touch this[int i]
        {
            get
            {
                if (i < 0 || i >= this.TouchCount)
                    throw new IndexOutOfRangeException($"Attempted to retrieve touch at index {i} but index must be between 0 and {this.TouchCount}.");

                return this.Touches[i];
            }
        }

        protected MultiTouchGesture(params Touch[] touches)
        {
            this.Touches = touches;
        }

        /// <summary>
        /// Generates a MultiTouchGesture from the provided touches.
        /// </summary>
        /// <param name="touches">Touches to generate the multi touch gesture from.</param>
        /// <returns>The Multi Touch Gesture that the provided touches are represented by.</returns>
        public static MultiTouchGesture FromTouches(params Touch[] touches)
        {
            Assert.IsTrue(touches.Length > 1, "A multi-touch gesture may only be generated from 2 touches or more.");

            // TODO:    Support for 4+ touches gestures. This is only an example project, not wanting to get too complicated here.
            //          For now, only ignore touches at indexes 3+.
            if (touches.Length > 3)
                return FromTouches(touches[0], touches[1], touches[2]);

            var singleTouchGestures = touches.Select(touch => SingleTouchGesture.FromTouch(touch)).ToArray();

            if (singleTouchGestures.Length == 3)
            {
                // If at least two of the three fingers are moving, then we're attempting to zoom in or out.
                if (singleTouchGestures.Count(gesture => gesture is DragGesture) > 1)
                    return new ZoomGesture(touches[0], touches[1], touches[2]);

                // Otherwise, we just ignore third touch, because I don't see use cases for third touch without having to focus more
                // on this problem, which isn't very important for the overall solution. We get the point.
                return FromTouches(touches[0], touches[1]);
            }

            // If at least one of the moving finger is moving, then we're attempting to rotate the camera.
            if (singleTouchGestures.Any(gesture => gesture is DragGesture))
                return new RotateGesture(touches[0], touches[1]);

            // TODO: More two finger gestures...
            return null;
        }
    }
}
