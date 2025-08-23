using System.Collections.Generic;
using UnityEngine;

public abstract class GenericPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    private Queue<T> _pool = new();

    public T Get()
    {
        if (_pool.Count == 0)
            Expand();

        T instance = _pool.Dequeue();

        instance.gameObject.SetActive(true);

        return instance;
    }

    public void Return(T instance)
    {
        instance.gameObject.SetActive(false);

        _pool.Enqueue(instance);
    }

    private void Expand()
    {
        T instance = Instantiate(_prefab);

        _pool.Enqueue(instance);
    }
}
