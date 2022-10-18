using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Scripts.Logic.CameraRTS
{
    public class CursorFreezer : MonoBehaviour
    {
        /// w00f: the script shouldn't check itself on 'enabled'
        /// even if it's in property that works without GameObject.enabled.
        public Vector2 Drag => (enabled ? _fakeMousePosition : Input.mousePosition) - _previousFramePosition;

        private Vector2 _previousFramePosition;
        private Vector2 _fakeMousePosition;

        private void OnEnable()
        {
            _previousFramePosition = _fakeMousePosition = Input.mousePosition;
            //Cursor.visible = false;
        }

        

        private void Update()
        {
            _fakeMousePosition += (Vector2)Input.mousePosition - _previousFramePosition;
        }

        private void LateUpdate()
        {
            _previousFramePosition = Input.mousePosition;

            //Mouse.current.WarpCursorPosition(_previousFramePosition);
            //InputState.Change(Mouse.current.position, _previousFramePosition);
        }

        //private void OnDisable() => Cursor.visible = true;
    }
}
