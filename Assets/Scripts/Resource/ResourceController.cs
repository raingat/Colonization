using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    private List<Resource> _busyResource = new();

    public void AddBusyResource(Resource resource)
    {
        _busyResource.Add(resource);
    }

    public void RemoveBusyResource(Resource resource)
    {
        _busyResource.Remove(resource);
    }

    public bool IsResourceBusy(Resource resource)
    {
        return _busyResource.Contains(resource);
    }
}
