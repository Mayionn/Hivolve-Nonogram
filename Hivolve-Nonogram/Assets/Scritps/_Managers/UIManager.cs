using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : SingletonDestroyable<UIManager>
{
    public Canvas Screen;

    private Hud gameHud;
    private Menu_Main menuMain;
    private Menu_EndlessMode endlessMode;
    private Menu_TimeAttack timeAttackMode;

    private void Start()
    {
        //----- If its the menu
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            endlessMode = GetComponent<Menu_EndlessMode>();
            menuMain = GetComponent<Menu_Main>();
            timeAttackMode = GetComponent<Menu_TimeAttack>();
        }
        else
        {
            gameHud = GetComponent<Hud>();
            gameHud.Init();
        }
    }

    public void OpenMenuUI() => menuMain.Init();
    public void CloseMenuUI() => menuMain.Terminate();

    public void OpenEndlessModeUI()
    {
        CloseMenuUI();
        endlessMode.Init();
    }
    public void OpenTimeAttackModeUI()
    {
        CloseMenuUI();
        timeAttackMode.Init();
    }

    public Text GetTimer()
    {
        return gameHud.Timer;
    }
}
