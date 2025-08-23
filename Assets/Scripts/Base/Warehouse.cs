using System;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    [SerializeField] private Base _base;

    public event Action<int> CountChanged;

    private int _currentResourceCount;

    public int CurrentResourceCount => _currentResourceCount;

    public void AddResource(Resource resource)
    {
        _currentResourceCount++;

        CountChanged?.Invoke(_currentResourceCount);
    }

    public void RemoveResource(int count)
    {
        _currentResourceCount -= count;

        CountChanged?.Invoke(_currentResourceCount);
    }
}
