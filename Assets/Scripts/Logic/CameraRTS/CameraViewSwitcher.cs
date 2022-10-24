using System.Collections;
using Additional;
using UnityEngine;

namespace Logic.CameraRTS
{
    [RequireComponent(typeof(Camera))]
    public class CameraViewSwitcher : MonoBehaviour
    {
        [SerializeField]
        private FollowView[] _views;
        [SerializeField]
        private float _transitionDuration;

        private Camera _camera;
        private int _currentViewIndex;
        private Coroutine _transition = null;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _views = GetComponents<FollowView>();
        }

        private void Start()
        {
            bool hasEnabled = false;
            
            for (int i = 0; i < _views.Length; i++)
            {
                if (_views[i].enabled)
                {
                    if (hasEnabled)
                        throw new UnityException("Camera game object must have only one follow view component enabled");

                    // define initial view
                    hasEnabled = true;
                    _currentViewIndex = i;
                }
            }
        }

        private void Update()
        {
            if (Input.mouseScrollDelta.y == -1)
                TryStartTransition(false);
            else
            if (Input.mouseScrollDelta.y == 1)
                TryStartTransition(true);
        }

        private void TryStartTransition(bool reversed = false)
        {
            int next = _currentViewIndex + (reversed ? -1 : 1);
            if (next.IsOutOfBounds(0, _views.Length - 1) == true)
                return;

            if (_transition != null) // interrupt current transition
                StopCoroutine(_transition);
            else
            if (reversed && _camera.fieldOfView != _views[_currentViewIndex].FovBounds.x)
                return; // can't go down
            else
            if (!reversed && _camera.fieldOfView != _views[_currentViewIndex].FovBounds.y)
                return; // can't go up

            foreach (FollowView follower in _views)
                follower.enabled = false;

            _currentViewIndex = next;
            _transition = StartCoroutine(AnimatableMove(reversed));
        }

        private IEnumerator AnimatableMove(bool reversed)
        {
            FollowView pursuer = _views[_currentViewIndex];

            Vector3 initialPosition = transform.position;
            Quaternion initialRotation = transform.rotation;
            float initialFov = _camera.fieldOfView;

            float targetFov = reversed ? pursuer.FovBounds.y : pursuer.FovBounds.x;

            // movement to a specific point in a given time
            float i = 1f / _transitionDuration * Time.fixedDeltaTime;

            for (float t = 0; t <= 1f; t += i)
            {
                yield return new WaitForFixedUpdate();
                pursuer.CalcExpectedTransform(out Vector3 targetPosition, out Quaternion targetRotation);

                transform.SetPositionAndRotation(
                    Vector3.Lerp(initialPosition, targetPosition, t),
                    Quaternion.Lerp(initialRotation, targetRotation, t));

                _camera.fieldOfView = Mathf.Lerp(initialFov, targetFov, t);
            }

            _transition = null;
            pursuer.enabled = true;
        }
    }
}
