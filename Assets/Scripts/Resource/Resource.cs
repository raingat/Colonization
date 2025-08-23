using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Collected;

    public void Collect()
    {
        Collected?.Invoke(this);
    }
}
