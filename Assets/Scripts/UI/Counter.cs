using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private Warehouse _warehouse;

    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _warehouse.CountChanged += ChangeValue;
    }

    private void OnDisable()
    {
        _warehouse.CountChanged -= ChangeValue;
    }

    private void ChangeValue(int count)
    {
        text.text = count.ToString();
    }
}
