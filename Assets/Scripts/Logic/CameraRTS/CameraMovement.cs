using UnityEngine;

namespace Logic.CameraRTS
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

        [Header("Mouse movement")]
        [SerializeField]
        private bool _isWheelMoveInversionEnabled;
        [SerializeField]
        private Vector2 _outOfWindowOffset;

        [Header("Zooming")]
        [SerializeField]
        private Vector2 _fieldOfViewBounds;

        private CursorFreezer _freezer;
        private float FovFactor => _fieldOfViewBounds.y == 0 ? 1f : _isometricCamera.fieldOfView / _fieldOfViewBounds.y;

        private bool IsIsometricEnabled
        {
            get
            {
                if (_thirdPersonCamera != null)
                {
                    if (Input.GetKeyUp(KeyCode.C))
                    {
                        _thirdPersonCamera.enabled = _isometricCamera.enabled;
                        _isometricCamera.enabled = !_isometricCamera.enabled;
                    }
                }

                return _isometricCamera.enabled;
            }
        }

        private bool IsCursorFreezed => _freezer != null && (_freezer.enabled = Input.GetKey(KeyCode.Mouse2));

        private bool IsCursorOffScreen => IsNumberOutOfBounds(Input.mousePosition.x, 0, Screen.width,  _outOfWindowOffset.x) ||
                                          IsNumberOutOfBounds(Input.mousePosition.y, 0, Screen.height, _outOfWindowOffset.y);

        private void Start() => _freezer = gameObject.AddComponent<CursorFreezer>();

        private void LateUpdate()
        {
            if (IsIsometricEnabled)
                IsometricUpdate();
        }

        private void IsometricUpdate()
        {
            Move(GetInputAxes(), _axisMovementVelocity);

            if (IsCursorOffScreen)
                Move(GetMouseFromCenterDirection(), _mouseMovementVelocity);
            else
            if (IsCursorFreezed)
                Move(_freezer.DragDirection, _mouseMovementVelocity * (_isWheelMoveInversionEnabled ? -1 : 1));

            if (Input.mouseScrollDelta.y != 0)
                Zoom(Input.mouseScrollDelta.y, _scrollingVelocity);
        }

        private void Move(Vector2 direction, float velocity) =>
            _isometricCamera.transform.position += velocity * Time.deltaTime * FovFactor * new Vector3(direction.x, 0, direction.y);

        private void Zoom(float direction, float velocity) => 
            _isometricCamera.fieldOfView = Mathf.Clamp(_isometricCamera.fieldOfView - velocity * Time.deltaTime * Mathf.Clamp(direction, -1f, 1f), _fieldOfViewBounds.x, _fieldOfViewBounds.y);

        private static bool IsNumberOutOfBounds(float number, float min, float max, float offset) => number < min + offset || number > max - offset;
        private static Vector2 GetInputAxes() => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        private static Vector2 GetMouseFromCenterDirection() => new Vector2(Input.mousePosition.x - Screen.width / 2f, Input.mousePosition.y - Screen.height / 2f).normalized;
    }

}
