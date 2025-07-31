using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnButton_InGame
{
    public void OnPauseButton()
    {
        Debug.Log("Pause Button Clicked");
    }
    public void OnContinueButton()
    {
        Debug.Log("Continue Button Clicked");
    }
    public void OnRestartButton()
    {
        Debug.Log("Restart Button Clicked");
    }
    public void OnExitButton()
    {
        Debug.Log("Exit Button Clicked");
    }
}
