using UnityEngine;

/// w00f: always use namespaces, beware type names overriding: Logic.Camera(RTS) and UnityEngine.Camera
namespace Assets.Scripts.Logic.CameraRTS
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private Camera _isometricCamera;
        [SerializeField]
        private Camera _thirdPersonCamera;

        [Header("Velocities setup")]
        [SerializeField]
        private float _axisMovementVelocity;
        [SerializeField]
        private float _mouseMovementVelocity;
        [SerializeField]
        private float _scrollingVelocity;

        [Header("Zooming")]
        [SerializeField]
        private Vector2 _fieldOfViewBand;

        private CursorFreezer _freezer;
        private float FovFactor => _isometricCamera.fieldOfView / _fieldOfViewBand.y;

        private bool IsIsometricEnabled
        {
            get
            {
                /// w00f: Always use the parentheses if other 'if' has them.
                if (_thirdPersonCamera != null)
                {
                    if (Input.GetKeyUp(KeyCode.C))
                    {
                        _thirdPersonCamera.enabled = _isometricCamera.enabled;
                        /// w00f: boolean check changes the state of enabled ?
                        _isometricCamera.enabled = !_isometricCamera.enabled;
                    }
                }

                return _isometricCamera.enabled;
            }
        }

        /// w00f: Separate the switch and check logics.
        private bool IsCursorFreezed => _freezer != null && (_freezer.enabled = Input.GetKey(KeyCode.Mouse2));

        /// w00f: Add offset bounds: x < 0 + xBound || x > width - xbound
        private bool IsCursorOffScreen => Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
                                          Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height;

        private void Start() => _freezer = gameObject.AddComponent<CursorFreezer>();

        private void LateUpdate()
        {
            /// w00f: Separate the switch and check logics.
            if (IsIsometricEnabled)
                IsometricUpdate();
        }

        private void IsometricUpdate()
        {
            Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 
                 _axisMovementVelocity);

            /// w00f: Please, normalize all direction's vectors.
            if (IsCursorOffScreen)
                Move(Input.mousePosition.x - Screen.width / 2f, Input.mousePosition.y - Screen.height / 2f,
                     _mouseMovementVelocity);
        

            /// w00f: separate cursorFreezed and outOfWindows mouse logic
            /// Add a mouseInvertion int = invert ? -1 : 1;
            if (IsCursorFreezed)
                Move(_freezer.Drag.x, _freezer.Drag.y,
                     _mouseMovementVelocity);
            /// else:
            if (Input.mouseScrollDelta.y != 0)
                Zoom(Input.mouseScrollDelta.y,
                     _scrollingVelocity);
        }

        /// w00f: first Vector3.Normalized() then Vector3.ClampMagnitude.
        private void Move(float x, float y, float velocity) =>
            _isometricCamera.transform.position += velocity * Time.deltaTime * FovFactor * Vector3.ClampMagnitude(new Vector3(x, 0, y), 1);

        /// w00f: parameter name isn't value, but 'zoomerDirection' for example.
        private void Zoom(float value, float velocity) => 
            _isometricCamera.fieldOfView = Mathf.Clamp(_isometricCamera.fieldOfView - velocity * Time.deltaTime * Mathf.Clamp(value, -1f, 1f), _fieldOfViewBand.x, _fieldOfViewBand.y);
    }

}
