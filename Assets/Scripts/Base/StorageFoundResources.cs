using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Scanner))]
public class StorageFoundResources : MonoBehaviour
{
    private int _maxNumberResources = 5;

    private int _currentNumberResource;

    private Scanner _scanner;

    private ResourceController _resourceController;

    private List<Resource> _resources = new();
    private Queue<Resource> _findResources = new();

    private void Awake()
    {
        _resourceController = FindAnyObjectByType<ResourceController>();

        _scanner = GetComponent<Scanner>();
    }

    private void OnEnable()
    {
        _scanner.ResourceFound += AddResourceInQueue;
    }

    private void OnDisable()
    {
        _scanner.ResourceFound -= AddResourceInQueue;
    }

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
        _resourceController.RemoveBusyResource(resource);

        _resources.Remove(resource);
        _currentNumberResource--;
    }

    private void AddResourceInQueue(Resource resource)
    {
        if (_resources.Contains(resource))
            return;

        if (_findResources.Contains(resource))
            return;

        if (_currentNumberResource >= _maxNumberResources)
            return;

        if (_resourceController.IsResourceBusy(resource))
            return;

        _resourceController.AddBusyResource(resource);

        _findResources.Enqueue(resource);
        _currentNumberResource++;
    }
}
