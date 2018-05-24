using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Called when player is about to enter a new stage
/// </summary>
public class BeforeChangeToNextStageEvent : Event
{
    public int CurSceneBuildIndex;

    public int NextSceneBuildIndex;

    public BeforeChangeToNextStageEvent (int curBuildIndex, int nextBuildIndex)
    {
        CurSceneBuildIndex = curBuildIndex;
        NextSceneBuildIndex = nextBuildIndex;
    }
}