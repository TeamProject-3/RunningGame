using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public interface IUiUpdate_InGame
{
    public void UpdateCoinText()
    {
        Debug.Log("Coin Updated: ");
    }
    public void UpdateStarText()
    {
        Debug.Log("Star Updated: " );
    }
    public void UpdateProgressSlider()
    {
        Debug.Log("Progress Updated: " );
    }
    public void UpdateHighScoreText()
    {
        Debug.Log("High Score Updated: ");
    }
    public void UpdateMyScoreText()
    {
        Debug.Log("My Score Updated: " );
    }
    public void UpdateMapNameText()
    {
        Debug.Log("Map Name Updated: " );
    }
}
