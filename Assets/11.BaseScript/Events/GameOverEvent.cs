using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Raise this event when game over
/// </summary>
public class GameOverEvent : Event
{
    public string CurrentSceneName;

    public GameOverEvent (string curSceneName)
    {
        CurrentSceneName = curSceneName;
    }

}