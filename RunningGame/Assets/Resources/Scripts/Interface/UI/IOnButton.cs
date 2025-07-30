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

    public void OnStartButton()
    {
        Debug.Log("Start Button Clicked");
    }
    public void OnStageSelectButton()
    {
        Debug.Log("Start Button Clicked");
    }

    public void OnNextStageButton()
    {
        Debug.Log("Next Stage Button Clicked");
    }
    public void OnPrevStageButton()
    {
        Debug.Log("Previous Stage Button Clicked");
    }
    public void OnRunButton()
    {
        Debug.Log("Run Button Clicked");
    }

    public void OnOutShopButton()
    {
        Debug.Log("Out Shop Button Clicked");
    }
}
