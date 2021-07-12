using UnityEngine;

namespace Assets.Scripts.Gestures
{
    public class RotateGesture : MultiTouchGesture
    {
        /// <summary>
        /// The rotation represented by this gesture, in EulerAngles.
        /// </summary>
        public Vector3 EulerAnglesValue { get; }

        public RotateGesture(Touch touch0, Touch touch1)
            : base(touch0, touch1)
        {
            Vector2 p0, p1, u0, u1;

            if (touch0.position.y < touch1.position.y)
            {
                p0 = touch1.position;
                p1 = touch0.position;
                u0 = touch1.position + touch1.deltaPosition;
                u1 = touch0.position + touch0.deltaPosition;
            }
            else
            {
                p0 = touch0.position;
                p1 = touch1.position;
                u0 = touch0.position + touch0.deltaPosition;
                u1 = touch1.position + touch1.deltaPosition;
            }

            var delta0 = Mathf.Abs(p0.magnitude - u0.magnitude);
            var delta1 = Mathf.Abs(p1.magnitude - u1.magnitude);

            var screenDeltas = delta1 > delta0
                ? new Vector2(p1.x - u1.x, p1.y - u1.y)
                : -new Vector2(p0.x - u0.x, p0.y - u0.y);

            this.EulerAnglesValue = new Vector3(screenDeltas.y, -screenDeltas.x, 0) / 1800f;
        }
    }
}
