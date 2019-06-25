using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public GameObject UI;
    public GameObject ResetSquares;
    public GameObject FillSquares;
    public GameObject LeaveGame;
    public Text Timer;

    public void Init()
    {
        ResetSquares.SetActive(true);
        FillSquares.SetActive(true);
        LeaveGame.SetActive(true);

        switch (PropertiesManager.Instance.GameMode)
        {
            case Enums.GameMode.Singleplayer:
                break;
            case Enums.GameMode.Endless:
                Timer.text = System.String.Empty;
                break;
            case Enums.GameMode.TimeAttack:
                break;
            default:
                break;
        }
    }
}
