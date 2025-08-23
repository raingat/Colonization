using System.Collections;
using UnityEngine;

public class ResourceSpawner : GenericSpawner<Resource>
{
    [SerializeField] private float _waitTime;

    [SerializeField] private int _maxCount;

    [SerializeField] private float _maxCoordinateX;
    [SerializeField] private float _minCoordinateX;
    [SerializeField] private float _maxCoordinateZ;
    [SerializeField] private float _minCoordinateZ;
    [SerializeField] private float _coordinateY;

    private int _currentCount;

    private void Start()
    {
        StartCoroutine(CreateObject());
    }

    protected override void HandleObject(Resource resource)
    {
        float randomCoordinateX = Random.Range(_minCoordinateX, _maxCoordinateX);
        float randomCoordinateZ = Random.Range(_minCoordinateZ, _maxCoordinateZ);

        resource.transform.position = new Vector3(randomCoordinateX, _coordinateY, randomCoordinateZ);

        resource.Collected += ReturnToPool;

        _currentCount++;
    }

    protected override void ReturnToPool(Resource resource)
    {
        _currentCount--;

        resource.Collected -= ReturnToPool;

        base.ReturnToPool(resource);
    }

    private IEnumerator CreateObject()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_waitTime);

        while (enabled)
        {
            yield return waitForSeconds;

            if (_currentCount < _maxCount)
            {
                Spawn();
            }
        }
    }
}
