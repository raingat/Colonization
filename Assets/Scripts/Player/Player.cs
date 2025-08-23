using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private LayerMask _enableLayerMask;

    private InputReader _inputReader = new();

    private Ray ray;
    private RaycastHit _raycastHit;

    private Base _base;

    private void FixedUpdate()
    {
        ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_inputReader.IsLeftMouseButton() && Physics.Raycast(ray, out _raycastHit, Mathf.Infinity, _enableLayerMask))
        {
            if (_raycastHit.transform.TryGetComponent(out Base target))
                _base = target;

            if (_raycastHit.transform.TryGetComponent(out Ground _) && _base != null)
                _base.BuildFlag(_raycastHit.point);
        }
    }
}
