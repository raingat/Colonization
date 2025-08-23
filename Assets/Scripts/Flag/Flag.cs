using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public event Action Built;

    public void Disable()
    {
        Built?.Invoke();

        Destroy(gameObject);
    }
}
