using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject UI;
    public Canvas Screen;

    private Menu_EndlessMode endlessMode;

    private void Start()
    {
        endlessMode = GetComponent<Menu_EndlessMode>();
    }

    public void OpenMenuUI()
    {
        UI.SetActive(true);
    }

    public void OpenEndlessModeUI()
    {
        UI.SetActive(false);
        endlessMode.Init();
    }
}
