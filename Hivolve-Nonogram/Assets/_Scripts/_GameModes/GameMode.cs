using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

public abstract class GameMode : MonoBehaviour
{
    public GameProperties Properties;

    public abstract void Init();
    public abstract void SetMapProperties();
    public abstract void LeaveGame();
    public abstract IEnumerator MapCompletion();
}
