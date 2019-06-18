using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Structs;

public class AssetsManager : Singleton<AssetsManager>
{
    public List<Skin> Skins;
    public Skin ActiveSkin;
    public GameObject Button;
    public GameObject Line;

    private int spritesheetX_Count = 3;
    private int spritesheetY_Count = 3;

    public void Init()
    {
        for (int i = 0; i < Skins.Count; i++)
        {
            Skin skin = new Skin();

            skin.BlackHole         = GetSprite(Skins[i].Spritesheet, 0, 2);
            skin.Blank             = GetSprite(Skins[i].Spritesheet, 1, 2);
            skin.Line              = GetSprite(Skins[i].Spritesheet, 2, 2);
            skin.LineCompleted     = GetSprite(Skins[i].Spritesheet, 0, 1);
            skin.Multiplier2X      = GetSprite(Skins[i].Spritesheet, 1, 1);
            skin.Multiplier3X      = GetSprite(Skins[i].Spritesheet, 2, 1);
            skin.OnePoint          = GetSprite(Skins[i].Spritesheet, 0, 0);
            skin.TwoPoint          = GetSprite(Skins[i].Spritesheet, 1, 0);
            skin.MultiplierOverlay = GetSprite(Skins[i].Spritesheet, 2, 0);

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
