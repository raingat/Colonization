using UnityEngine;
using System.Collections.Generic;

public class StorageFoundResources : MonoBehaviour
{
    private int _maxNumberResources = 5;

    private int _currentNumberResource;

    private List<Resource> _resources = new();
    private Queue<Resource> _findResources = new();

    public Resource GetResource()
    {
        if (_findResources.Count == 0)
        {
            return null;
        }

        Resource resource = _findResources.Dequeue();

        if (_resources.Contains(resource))
        {
            return null;
        }
        else
        {
            _resources.Add(resource);

            return resource;
        }
    }

    public void RemoveResource(Resource resource)
    {
        _resources.Remove(resource);
        _currentNumberResource--;
    }

    public void TryAddResourceInQueue(Resource resource)
    {
        if (_resources.Contains(resource))
            return;

        if (_findResources.Contains(resource))
            return;

        if (_currentNumberResource >= _maxNumberResources)
            return;

        _findResources.Enqueue(resource);
        _currentNumberResource++;
    }
}
