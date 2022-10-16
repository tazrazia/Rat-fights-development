using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Виды камер")]
    [SerializeField]
    private Camera _isometricCamera;
    [SerializeField]
    private Camera _thirdPersonCamera;

    [Header("Настройки")]
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _scrollSpeed;
    [SerializeField]
    private Vector2 _fieldOfViewBand;

    [Space]
    [SerializeField]
    private CursorFreezer _freezer;

    private float MoveKf => _isometricCamera.fieldOfView / _fieldOfViewBand.y * _moveSpeed * Time.deltaTime;
    private float ScrollKf => _scrollSpeed * Time.deltaTime;

    private bool IsIsometricEnabled
    {
        get
        {
            if (_thirdPersonCamera != null)
                if (Input.GetKeyUp(KeyCode.C))
                {
                    _thirdPersonCamera.enabled = _isometricCamera.enabled;
                    _isometricCamera.enabled = !_isometricCamera.enabled;
                }

            return _isometricCamera.enabled;
        }
    }

    private bool IsCursorFreezed => _freezer != null && (_freezer.enabled = Input.GetKey(KeyCode.Mouse2));
    private bool IsCursorOffScreen => Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
                                      Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height;

    private void Update()
    {
        if (IsIsometricEnabled)
            IsometricUpdate();
    }

    private void IsometricUpdate()
    {
        // перемещение стрелками
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // перемещение мышью
        if (IsCursorOffScreen)
            Move(Input.mousePosition.x - Screen.width / 2f, Input.mousePosition.y - Screen.height / 2f);

        // перемещение колесиком
        if (IsCursorFreezed)
            Move(_freezer.Drag.x, _freezer.Drag.y);

        // приближение
        if (Input.mouseScrollDelta.y != 0)
            Zoom(Input.mouseScrollDelta.y);
    }

    private void Move(float x, float y) =>
        _isometricCamera.transform.position += MoveKf * Vector3.ClampMagnitude(new Vector3(x, 0, y), 1);

    private void Zoom(float value) => 
        _isometricCamera.fieldOfView = Mathf.Clamp(_isometricCamera.fieldOfView - ScrollKf * Mathf.Clamp(value, -1f, 1f), _fieldOfViewBand.x, _fieldOfViewBand.y);
}
