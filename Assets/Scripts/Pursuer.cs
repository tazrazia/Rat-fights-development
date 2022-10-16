using UnityEngine;

public class Pursuer : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    private Vector3 _lastTargetPosition;

    private void Start()
    {
        _lastTargetPosition = _target.position;
    }

    private void LateUpdate()
    {
        if (_lastTargetPosition == _target.position)
            return;

        transform.position += _target.position - _lastTargetPosition;
        _lastTargetPosition = _target.position;
    }
}
