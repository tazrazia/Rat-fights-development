using Additional;
using UnityEngine;

namespace Logic.CameraRTS
{
    [RequireComponent(typeof(Camera))]
    public class MoveableFollowView : FollowView
    {
        [Space]
        [Header("Movement setup")]
        [SerializeField]
        private float _axisMovementVelocity;
        [SerializeField]
        private float _mouseMovementVelocity;
        [SerializeField]
        private float _rotationVelocity;

        [Space]
        [SerializeField]
        private bool _isWheelMoveInversionEnabled;
        [SerializeField]
        private Vector2 _outOfWindowOffset;

        private CursorFreezer _freezer;

        private bool HasToFollow { get; set; }
        private float FovFactor => FovBounds.y == 0 ? 1f : _camera.fieldOfView / FovBounds.y;
        private bool IsCursorFreezed => _freezer != null && (_freezer.enabled = Input.GetKey(KeyCode.Mouse2));
        private bool IsCursorOffScreen => Input.mousePosition.x.IsOutOfBounds(_outOfWindowOffset.x, Screen.width - _outOfWindowOffset.x) ||
                                          Input.mousePosition.y.IsOutOfBounds(_outOfWindowOffset.y, Screen.height - _outOfWindowOffset.y);

        private void Start()
        {
            SetInitialViewState();
            _freezer = gameObject.AddComponent<CursorFreezer>();
        }

        private void OnDisable() => HasToFollow = false;

        private void Update()
        {
            ZoomIfScrolled();

            if (Input.GetKeyDown(KeyCode.C))
                HasToFollow = !HasToFollow;
        }

        private void LateUpdate()
        {
            Move(GetInputAxes(), _axisMovementVelocity);

            if (IsCursorFreezed)
                Move(_freezer.DragDirection, _mouseMovementVelocity * (_isWheelMoveInversionEnabled ? -1 : 1));
            else if (IsCursorOffScreen)
                Move(GetMouseFromCenterDirection(), _mouseMovementVelocity);

            if (Input.GetKey(KeyCode.R))
                Rotate(-1f, _rotationVelocity);
            else
            if (Input.GetKey(KeyCode.T))
                Rotate(1f, _rotationVelocity);

            if (HasToFollow)
                UpdateFollowedView();
        }

        private void Move(Vector2 direction, float velocity)
        {
            if (direction == Vector2.zero)
                return;

            HasToFollow = false;
            transform.position += velocity * Time.deltaTime * FovFactor * new Vector3(direction.x, 0, direction.y);
        }

        private void Rotate(float direction, float velocity)
        {
            if (direction == 0f)
                return;

            HasToFollow = false;
            transform.Rotate(0f, 0f, velocity * Time.deltaTime * direction);
        }

        private static Vector2 GetInputAxes() => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        private static Vector2 GetMouseFromCenterDirection() => new Vector2(Input.mousePosition.x - Screen.width / 2f, Input.mousePosition.y - Screen.height / 2f).normalized;
    }
}
