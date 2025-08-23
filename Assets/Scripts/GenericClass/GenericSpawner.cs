using UnityEngine;

public abstract class GenericSpawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private GenericPool<T> _pool;

    public T Spawn()
    {
        T instance = _pool.Get();

        HandleObject(instance);

        return instance;
    }

    protected virtual void ReturnToPool(T instance)
    {
        _pool.Return(instance);
    }

    protected abstract void HandleObject(T instance);
}
