using DG.Tweening;
using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private float _travelSpeed;
    [SerializeField] private float _deliverySpeed;

    [SerializeField] private float _timeRotate;

    private BaseSpawner _baseSpawner;

    private Transform _collectionPoint;

    private Resource _currentResource;

    private float _heightSpawnNewBase = 0.5f;

    private bool _isTransferResource;

    public bool IsTransferResource => _isTransferResource;

    public event Action<Bot, Base> TransferringToNewBase;

    private void Awake()
    {
        _isTransferResource = false;
    }

    public void Initialize(BaseSpawner baseSpawner)
    {
        _baseSpawner = baseSpawner;
    }

    public void SetCollectionPoint(Transform point)
    {
        _collectionPoint = point;
    }

    public void CollectResource(Resource resource)
    {
        _currentResource = resource;

        Vector3 positionResource = resource.transform.position;

        CalculateWalkTime(positionResource, out float timeWalkToResource, out float timeWalkToWarehouse);

        DeliverResource(positionResource, resource, timeWalkToResource, timeWalkToWarehouse);
    }

    public Resource GiveAwayResource()
    {
        Resource resource = _currentResource;

        resource.transform.SetParent(null);

        _isTransferResource = false;

        _currentResource = null;

        return resource;
    }

    public void BuildNewBase(Flag flag)
    {
        Vector3 positionFlag = flag.transform.position;

        CalculateWalkTime(positionFlag, out float timeWalkToResource, out float timeWalkToWarehouse);

        DeliveryToBuildPoint(positionFlag, flag, timeWalkToResource, timeWalkToWarehouse);
    }

    private void DeliveryToBuildPoint(Vector3 positionFlag, Flag flag, float timeWalkToResource, float timeWalkToWarehouse)
    {
        Sequence _sequence = DOTween.Sequence();

        Tween rotateToTargetTween = transform
            .DOLookAt(positionFlag, _timeRotate);

        Tween moveToTargetTween = transform
            .DOMove(positionFlag, timeWalkToResource)
            .SetEase(Ease.Linear)
            .OnComplete(() => ConstructsBase(flag));

        _sequence.Append(rotateToTargetTween);
        _sequence.Append(moveToTargetTween);
    }

    private void ConstructsBase(Flag flag)
    {
        flag.Disable();

        Base newBase = _baseSpawner.Spawn();

        newBase.transform.position = new Vector3(flag.transform.position.x, _heightSpawnNewBase, flag.transform.position.z);

        flag = null;

        TransferringToNewBase?.Invoke(this, newBase);
    }

    private void DeliverResource(Vector3 positionResource, Resource resource, float timeWalkToResource, float timeWalkToWarehouse)
    {
        Sequence _sequence = DOTween.Sequence();

        Tween rotateToTargetTween = transform
            .DOLookAt(positionResource, _timeRotate);

        Tween moveToTargetTween = transform
            .DOMove(positionResource, timeWalkToResource)
            .SetEase(Ease.Linear)
            .OnComplete(() => GrabResource(resource));

        Tween rotateToWarehouse = transform
            .DOLookAt(_collectionPoint.position, _timeRotate);

        Tween moveToWarehouse = transform
            .DOMove(_collectionPoint.position, timeWalkToWarehouse)
            .SetEase(Ease.Linear);

        _sequence.Append(rotateToTargetTween);
        _sequence.Append(moveToTargetTween);
        _sequence.Append(rotateToWarehouse);
        _sequence.Append(moveToWarehouse);
    }

    private void GrabResource(Resource resource)
    {
        resource.transform.SetParent(transform);

        _isTransferResource = true;
    }

    private void CalculateWalkTime(Vector3 positionResource, out float timeWalkToResource, out float timeWalkToWarehouse)
    {
        timeWalkToResource = (transform.position - positionResource).magnitude / _travelSpeed;

        timeWalkToWarehouse = (positionResource - _collectionPoint.position).magnitude / _deliverySpeed;
    }
}
