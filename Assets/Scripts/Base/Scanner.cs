using System;
using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _lengthZone;
    [SerializeField] private float _weightZone;
    [SerializeField] private float _heightZone;
    [SerializeField] private LayerMask _researchLayer;

    [SerializeField] private float _waitTime;

    private Collider[] _findCollider;

    public event Action<Resource> ResourceFound;

    private void Start()
    {
        StartCoroutine(ExploreArea());
    }

    private void ResearchResource()
    {
        _findCollider = Physics.OverlapBox(transform.position, new Vector3(_lengthZone, _heightZone, _weightZone), Quaternion.identity, _researchLayer);

        foreach (Collider collider in _findCollider)
        {
            if (collider.TryGetComponent(out Resource resource))
            {
                ResourceFound?.Invoke(resource);
            }
        }
    }

    private IEnumerator ExploreArea()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_waitTime);

        while (enabled)
        {
            yield return waitForSeconds;

            ResearchResource();
        }
    }
}
