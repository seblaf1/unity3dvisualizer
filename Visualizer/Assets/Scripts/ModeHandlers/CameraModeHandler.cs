using UnityEngine;
using UnityEngine.Assertions;
using Assets.Scripts.Gestures;
using Assets.Scripts.Types;

namespace Assets.Scripts.ModeHandlers
{
    public class CameraModeHandler : ModeHandler
    {
        private readonly Camera _camera;
        private readonly IReadOnlyReference<float> _sensitivity;
        private readonly Vector3 _cameraInitialPosition;
        private readonly Quaternion _cameraInitialRotation;

        //private const float HoldTimeInSecondsBeforeCameraReset = 3.5f;
        //private float _holdStartTime;

        public CameraModeHandler(Camera camera, IReadOnlyReference<float> sensitivity)
        {
            Assert.IsNotNull(camera, "Camera Mode Handler requires a non-null camera reference!");

            this._camera = camera;
            this._sensitivity = sensitivity;
            this._cameraInitialPosition = camera.transform.position;
            this._cameraInitialRotation = camera.transform.rotation;
        }

        protected override void HandleZoom(ZoomGesture gesture)
        {
            var zoomValue = -gesture.ZoomValue * this._sensitivity.Value;
            var translation = zoomValue * this._camera.transform.forward;

            this._camera.transform.position += translation;
        }

        protected override void HandleRotate(RotateGesture gesture)
        {
            var resultingEulerAngles = this._camera.transform.rotation.eulerAngles + gesture.EulerAnglesValue * this._sensitivity.Value;
            var resultingRotation = Quaternion.Euler(resultingEulerAngles);

            this._camera.transform.rotation = resultingRotation;
        }

        protected override void HandleDrag(DragGesture gesture)
        {
            var translation2D = gesture.TranslationResult * this._sensitivity.Value;
            var translation3D = this._camera.transform.rotation * translation2D;

            this._camera.transform.position += translation3D;
        }

        protected override void HandleHold(HoldGesture gesture)
        {
            //var elapsedTime = Time.time - this._holdStartTime;
            //if (elapsedTime < 0)
            //    return;

            //var remainingTime = HoldTimeInSecondsBeforeCameraReset - elapsedTime;
            //if (remainingTime < 2f)
            //{
            //    if (remainingTime <= 0f)
            //    {
            //        this._camera.transform.position = this._cameraInitialPosition;
            //        this._camera.transform.rotation = this._cameraInitialRotation;
            //        this._holdStartTime = Mathf.Infinity;

            //        Debugger.Instance.Hide();
            //    }

            //    Debugger.Instance.Show("Camera Reset", $"In: {remainingTime.ToString().Substring(0, 3)} seconds");
            //}
        }

        protected override void HandleStart(StartGesture start)
        {
            //this._holdStartTime = Time.time; 
        }

        protected override void HandleRelease(ReleaseGesture gesture)
        {
            //this._holdStartTime = Mathf.Infinity;
        }
    }
}
