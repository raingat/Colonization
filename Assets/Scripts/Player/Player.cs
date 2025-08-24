using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private LayerMask _enableLayerMask;

    private InputReader _inputReader = new();

    private Ray ray;
    private RaycastHit _raycastHit;

    private FlagSetter _flagSetter;

    private void Update()
    {
        ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_inputReader.IsLeftMouseButton() && Physics.Raycast(ray, out _raycastHit, Mathf.Infinity, _enableLayerMask))
        {
            if (_raycastHit.transform.TryGetComponent(out FlagSetter flagSetter))
            {
                _flagSetter = flagSetter;
            }

            if (_raycastHit.transform.TryGetComponent(out Ground _) && _flagSetter != null)
            {
                _flagSetter.Build(_raycastHit.point);
            }
        }
    }
}
