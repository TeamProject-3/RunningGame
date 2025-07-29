using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnButton
{

    public void OnShopButton()
    {
        Debug.Log("Shop Button Clicked");
    }
    public void OnGachaButton()
    {
        Debug.Log("Gacha Button Clicked");
    }

    public void OnAlbum()
    {
        Debug.Log("Album Button Clicked");
    }

    public void OnSettingButton()
    {
        Debug.Log("Setting Button Clicked");
    }
    public void OnPlayButton()
    {
        Debug.Log("Play Button Clicked");
    }
    public void OnSaveButton()
    {
        Debug.Log("Save Button Clicked");
    }
    public void HamburgerButton()
    {
        Debug.Log("Hamburger Button Clicked");
    }

}
