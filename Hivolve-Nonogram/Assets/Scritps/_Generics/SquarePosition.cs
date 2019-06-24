using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePosition : MonoBehaviour
{
    public Vector2 Position;

    public void SetPosition(int x, int y)
    {
        Position = new Vector2(x, y);
    }
}
