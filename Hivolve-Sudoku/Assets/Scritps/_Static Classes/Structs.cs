using System;
using UnityEngine;
using static Enums;

public static class Structs
{
    [Serializable]
    public struct GameProperties
    {
        public int SizeX;
        public int SizeY;
        public Density BlackHoles;
        public Density OnePointers;
        public Density TwoPointers;
        public Count Multipliers2X;
        public Count Multipliers3X;
    }

    //----- SKINS
    [Serializable]
    public struct Skin
    {
        [SerializeField] private string name;

        public Texture2D Spritesheet;
        [HideInInspector] public Sprite BlackHole;
        [HideInInspector] public Sprite Blank;
        [HideInInspector] public Sprite Line;
        [HideInInspector] public Sprite LineCompleted;
        [HideInInspector] public Sprite OnePoint;
        [HideInInspector] public Sprite TwoPoint;
        [HideInInspector] public Sprite Multiplier2X;
        [HideInInspector] public Sprite Multiplier3X;
        [HideInInspector] public Sprite MultiplierOverlay;
    }
}
