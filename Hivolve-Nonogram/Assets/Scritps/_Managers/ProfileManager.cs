using UnityEngine;
using static Structs;

public class ProfileManager : Singleton<ProfileManager>
{
    public Skin ActiveSkin;
    public int Currency;




    public void AddCurrency(int amount)
    {
        Currency += amount;
    }

    public void SetSkin(Skin skin)
    {
        this.ActiveSkin = skin;
    }
}
