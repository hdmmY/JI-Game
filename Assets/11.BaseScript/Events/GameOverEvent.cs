using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvent : Event
{
    public string CurrentSceneName;

    public GameOverEvent (string curSceneName)
    {
        CurrentSceneName = curSceneName;
    }

}