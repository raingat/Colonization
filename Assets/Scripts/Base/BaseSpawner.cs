using UnityEngine;

public class BaseSpawner : GenericSpawner<Base>
{
    [SerializeField] private StorageFoundResources _storageFoundResources;

    protected override void HandleObject(Base instance)
    {
        instance.Initialize(_storageFoundResources, this);
    }
}
