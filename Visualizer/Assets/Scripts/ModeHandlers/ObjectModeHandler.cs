using UnityEngine;
using UnityEngine.Assertions;
using Assets.Scripts.Types;
using Assets.Scripts.Gestures;

namespace Assets.Scripts.ModeHandlers
{
    public class ObjectModeHandler : ModeHandler
    {
        private readonly Camera _camera;
        private readonly IReadOnlyReference<GameObject> _objectInPreview;
        private readonly IReadOnlyReference<float> _sensitivity;

        public ObjectModeHandler(Camera camera, IReadOnlyReference<GameObject> objectInPreview, IReadOnlyReference<float> sensitivity)
        {
            this._camera = camera;
            this._objectInPreview = objectInPreview;
            this._sensitivity = sensitivity;
        }

        protected override void HandleZoom(ZoomGesture gesture)
        {
            Assert.IsNotNull(this._objectInPreview.Value, "Object Mode Handler shouldn't be the active handler if there's no object in preview!");

            var scaleValue = -gesture.ZoomValue * this._sensitivity.Value;
            var scaleModifier = Vector3.one * scaleValue / 1.4f;

            this._objectInPreview.Value.transform.localScale += scaleModifier;
        }

        protected override void HandleRotate(RotateGesture gesture)
        {
            Assert.IsNotNull(this._objectInPreview.Value, "Object Mode Handler shouldn't be the active handler if there's no object in preview!");

            var resultingEulerAngles = this._objectInPreview.Value.transform.rotation.eulerAngles + gesture.EulerAnglesValue * this._sensitivity.Value;
            var resultingRotation = Quaternion.Euler(resultingEulerAngles);

            this._objectInPreview.Value.transform.rotation = resultingRotation;
        }

        protected override void HandleDrag(DragGesture gesture)
        {
            Assert.IsNotNull(this._objectInPreview.Value, "Object Mode Handler shouldn't be the active handler if there's no object in preview!");

            var translation2D = -gesture.TranslationResult * this._sensitivity.Value;
            var translation3D = this._camera.transform.rotation * translation2D;

            this._objectInPreview.Value.transform.position += translation3D;
        }
    }
}
