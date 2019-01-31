using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    public MissionController missionController;
    private Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            missionController.CompletedMission(currentScene.name, 5400f);
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            missionController.BackToMainMenu();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            missionController.Reset(currentScene.name);
        }

    }
}
