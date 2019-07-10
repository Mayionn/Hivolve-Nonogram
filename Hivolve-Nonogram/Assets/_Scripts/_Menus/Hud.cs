using UnityEngine;
using UnityEngine.UI;
using static Structs;

public class Hud : MonoBehaviour
{
    public GameObject UI;
    public GameObject ResetSquares;
    public GameObject LeaveGame;

    public InfoTimeAttack TimeAttack;

    public void Init()
    {
        //ResetSquares.SetActive(true);
        //LeaveGame.SetActive(true);
        TimeAttack.TextTimer.text = System.String.Empty;
        TimeAttack.TextCurrency.text = System.String.Empty;
    }
}
