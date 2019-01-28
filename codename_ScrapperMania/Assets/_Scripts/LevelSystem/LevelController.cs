using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public List<Level> levels;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        levels = new List<Level>
        {
            new Level(0, "Introduction", false, 0, false),
            new Level(1, "Mission1", false, 0, true),
            new Level(2, "Mission2", false, 0, true),
            new Level(3, "Mission3", false, 0, true),
            new Level(4, "Mission4", false, 0, true),
            new Level(5, "Mission5", false, 0, true),
            new Level(6, "Mission6", false, 0, true),
            new Level(7, "Mission7", false, 0, true),
            new Level(8, "Mission8", false, 0, true),
            new Level(9, "Mission9", false, 0, true)
        };
    }

    public void StartLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void CompleteLevel(string levelName)
    {
        levels.Find(i => i.LevelName == levelName).Complete();
    }

    public void CompleteLevel(string levelName, int stars)
    {
        levels.Find(i => i.LevelName == levelName).Complete(stars);
    }
}
