using UnityEngine;

namespace Logic.CameraRTS
{
    public class CursorFreezer : MonoBehaviour
    {
        public Vector2 DragDirection => (_fakeMousePosition - _previousFramePosition).normalized;

        private Vector2 _previousFramePosition;
        private Vector2 _fakeMousePosition;

        private void OnEnable() => _previousFramePosition = _fakeMousePosition = Input.mousePosition;

        private void Update() => _fakeMousePosition += (Vector2)Input.mousePosition - _previousFramePosition;

        private void LateUpdate() => _previousFramePosition = Input.mousePosition;
    }
}
