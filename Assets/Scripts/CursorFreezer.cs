using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CursorFreezer : MonoBehaviour
{
    public Vector2 DragPosition => (Vector2)Input.mousePosition - _initialPosition;

    private Vector2 _initialPosition;
    //private Vector2 _fakeMousePosition;

    private void OnEnable() => _initialPosition = Input.mousePosition;
    //private void OnDisable() => ExpectedPosition = Input.mousePosition;

    private void LateUpdate()
    {
        Mouse.current.WarpCursorPosition(_initialPosition);
        InputState.Change(Mouse.current.position, _initialPosition);
    }

    //if (Input.GetKeyDown(KeyCode.Mouse2))
    //    _cursorFreezePosition = Input.mousePosition;

    //if (Input.GetKey(KeyCode.Mouse2))
    //{
    //    Move(Input.mousePosition.x - _cursorFreezePosition.x, Input.mousePosition.y - _cursorFreezePosition.y);
    //    FreezeCursor();
    //}
}
