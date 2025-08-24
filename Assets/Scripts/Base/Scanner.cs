using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _lengthZone;
    [SerializeField] private float _weightZone;
    [SerializeField] private float _heightZone;
    [SerializeField] private LayerMask _researchLayer;

    [SerializeField] private float _waitTime;

    [SerializeField] private StorageFoundResources _storageFoundResources;

    private Collider[] _findCollider;

    private void Start()
    {
        StartCoroutine(ExploreArea());
    }

    public void Initialize(StorageFoundResources storageFoundResources)
    {
        _storageFoundResources = storageFoundResources;
    }

    private void ResearchResource()
    {
        _findCollider = Physics.OverlapBox(transform.position, new Vector3(_lengthZone, _heightZone, _weightZone), Quaternion.identity, _researchLayer);

        foreach (Collider collider in _findCollider)
        {
            if (collider.TryGetComponent(out Resource resource))
            {
                _storageFoundResources.TryAddResourceInQueue(resource);
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
