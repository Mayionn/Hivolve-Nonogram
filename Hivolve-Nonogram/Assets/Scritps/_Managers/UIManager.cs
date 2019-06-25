using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonDestroyable<UIManager>
{
    public Canvas Screen;

    private Hud gameHud;
    private Menu_Main menuMain;
    private Menu_EndlessMode endlessMode;

    private void Start()
    {
        //----- If its the menu
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            endlessMode = GetComponent<Menu_EndlessMode>();
            menuMain = GetComponent<Menu_Main>();
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


}
