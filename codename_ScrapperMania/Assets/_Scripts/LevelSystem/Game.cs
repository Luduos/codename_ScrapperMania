using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private LevelController levelController;
    private void Awake()
    {
        levelController = FindObjectOfType<LevelController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            levelController.CompleteLevel("First Theft", 5400);
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            levelController.StartLevel("MainMenu");
        }
    }
}
