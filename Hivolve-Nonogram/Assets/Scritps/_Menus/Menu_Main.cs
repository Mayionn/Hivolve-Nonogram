using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Main : MonoBehaviour
{
    public GameObject UI;

    public void Init()
    {
        UI.SetActive(true);
    }
    public void Terminate()
    {
        UI.SetActive(false);
    }
}
