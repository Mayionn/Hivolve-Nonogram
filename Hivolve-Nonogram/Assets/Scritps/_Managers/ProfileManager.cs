using UnityEngine;
using static Structs;

public class ProfileManager : Singleton<ProfileManager>
{
    public Skin ActiveSkin;



    public void SetSkin(Skin skin)
    {
        this.ActiveSkin = skin;
    }
}
