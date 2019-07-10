using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endless : GameMode
{
    public override void Init()
    {
        Properties = PropertiesManager.Instance.CustomProperties;
    }

    public override void LeaveGame()
    {
    }

    public override IEnumerator MapCompletion()
    {
        //---- Play Add
        if (AdManager.Instance.CheckIfReady())
        {
            AdManager.Instance.Display_InterstitialAD();
        }

        ProfileManager.Instance.AddCurrency(PropertiesManager.Instance.CustomMapReward);

        GameManager.Instance.FadeOutUI();

        yield return new WaitForSeconds(1.5f);

        GameManager.Instance.GenerateNextMap();
    }

    public override void SetMapProperties()
    {
        Properties = PropertiesManager.Instance.CustomProperties;
    }
}
