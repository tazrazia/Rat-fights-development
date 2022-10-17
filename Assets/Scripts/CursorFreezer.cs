using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CursorFreezer : MonoBehaviour
{
    public Vector2 Drag => (enabled ? _fakeMousePosition : Input.mousePosition) - _initialPosition;

    private Vector2 _initialPosition;
    private Vector2 _fakeMousePosition;

    private void OnEnable()
    {
        _initialPosition = _fakeMousePosition = Input.mousePosition;
        //Cursor.visible = false;
    }

    private void Update() => _fakeMousePosition += (Vector2)Input.mousePosition - _initialPosition;

    private void LateUpdate()
    {
        Mouse.current.WarpCursorPosition(_initialPosition);
        InputState.Change(Mouse.current.position, _initialPosition);
    }

    //private void OnDisable() => Cursor.visible = true;
}
