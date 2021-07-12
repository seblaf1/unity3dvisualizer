using UnityEngine;

namespace Assets.Scripts.Gestures
{
    public class ZoomGesture : MultiTouchGesture
    {
        /// <summary>
        /// The zoom value is negative if fingers for this gesture are getting closer.
        /// On the contrary, it is positive if fingers are getting further appart.
        /// 
        /// The absolute value is greater the quicker the fingers are moving.
        /// </summary>
        public float ZoomValue { get; }

        public ZoomGesture(Touch touch0, Touch touch1, Touch touch2)
            : base(touch0, touch1, touch2)
        {
            Vector2 LastFrame(Touch touch) => touch.position + touch.deltaPosition;

            // Compute Zoom Value. To find zoom value, we calculate a triangle area represented by last frame's finger positions
            // and compare it to this frame's.
            var currentFrame = ComputeTriangleArea(touch0.position, touch1.position, touch2.position);
            var lastFrame = ComputeTriangleArea(LastFrame(touch0), LastFrame(touch1), LastFrame(touch2));

            this.ZoomValue = (currentFrame - lastFrame) / 9000000f;
        }

        private static float ComputeTriangleArea(Vector2 p1, Vector2 p2, Vector3 p3)
        {
            // To understand this formula, which computes a triangle area from 3 points,
            // see https://www.cuemath.com/geometry/area-of-triangle-in-coordinate-geometry/
            return 0.5f * Mathf.Abs(p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y));
        }
    }
}
