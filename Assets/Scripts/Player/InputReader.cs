using UnityEngine;

public class InputReader
{
    private const int NumberLeftMouseButton = 0;

    public bool IsLeftMouseButton()
    {
        return Input.GetMouseButtonDown(NumberLeftMouseButton);
    }
}
