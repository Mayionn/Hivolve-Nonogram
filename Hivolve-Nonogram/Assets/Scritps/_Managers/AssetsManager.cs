using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Structs;

public class AssetsManager : MonoBehaviour
{
    public List<Skin> Skins;
    [HideInInspector] public Skin ActiveSkin;

    public GameObject Button;
    public GameObject Line;
    public GameObject StarLine;

    private int spritesheetX_Count = 4;
    private int spritesheetY_Count = 4;

    public void Init()
    {
        for (int i = 0; i < Skins.Count; i++)
        {
            Skin skin = new Skin();

            skin.Spritesheet = Skins[i].Spritesheet;
            skin.Blank             = GetSprite(Skins[i].Spritesheet, 0, 3);
            skin.OnePoint          = GetSprite(Skins[i].Spritesheet, 1, 3);
            skin.TwoPoint          = GetSprite(Skins[i].Spritesheet, 2, 3);
            skin.BlackHole         = GetSprite(Skins[i].Spritesheet, 3, 3);
            skin.Line              = GetSprite(Skins[i].Spritesheet, 0, 2);
            skin.LineCompleted     = GetSprite(Skins[i].Spritesheet, 1, 2);
            skin.Multiplier        = GetSprite(Skins[i].Spritesheet, 2, 2);
            skin.MultiplierOverlay = GetSprite(Skins[i].Spritesheet, 3, 2);
            skin.Background = Skins[i].Background;
            Skins[i] = skin;
        }

        ActiveSkin = Skins[0];
    }

    private Sprite GetSprite(Texture2D spritesheet, int posX, int posY)
    {
        float offSetX = spritesheet.width / spritesheetX_Count;
        float offSetY = spritesheet.height / spritesheetY_Count;

        return Sprite.Create(spritesheet, new Rect(offSetX * posX, offSetY * posY, offSetX, offSetY), new Vector2(0.5f, 0.5f), 1f);
    }
}
