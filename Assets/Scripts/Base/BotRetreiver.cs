using System;
using UnityEngine;

public class BotRetreiver : MonoBehaviour
{
    private Base _base;

    public event Action<Bot> BotCome;

    private void Awake()
    {
        _base = transform.parent.GetComponent<Base>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bot bot))
        {
            if (_base.IsMineBot(bot) && bot.IsTransferResource)
            {
                BotCome?.Invoke(bot);
            }
        }
    }
}
