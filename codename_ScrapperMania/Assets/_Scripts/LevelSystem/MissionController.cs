using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[CreateAssetMenu(menuName = "Mission Controller")]
public class MissionController : SingletonScriptableObject<MissionController>
{
    public List<Mission> missions;
    public String mainMenuSceneName;

    public void CompletedMission(string sceneName)
    {
        int index = missions.FindIndex(i => i.missionName == sceneName);
        missions[index].Complete();
        missions[index + 1].Unlock();
    }

    public void CompletedMission(string sceneName, float score)
    {
        int index = missions.FindIndex(i => i.missionName == sceneName);
        missions[index].Complete(score);
        missions[index + 1].Unlock();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Reset(string sceneName)
    {
        missions.Find(i => i.missionName == sceneName).Reset();
    }

    public List<Mission> GetMissions()
    {
        return missions;
    }
}
