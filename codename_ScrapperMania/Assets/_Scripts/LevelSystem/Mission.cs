using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Mission")]
public class Mission : ScriptableObject
{
    [Header("General Attributes")]
    [Tooltip("Has to be the same as the name of the Scene")]
    public string missionName = "Default";
    public bool isLocked = true;
    public bool isCompleted = false;
    public float missionScore = 0f;

    private int missionID = -1;
    public int MissionID { get { return missionID; } }

    [Header("UI Attributes")]
    public Sprite backgroundSprite;
    [TextArea]
    public string missionDescription = "Default description";

    [SerializeField] private Sprite lockImage;

    public void Initialize()
    {
        try
        {
            SceneManager.LoadScene(missionName);
        } catch (Exception e)
        {
            Debug.LogException(e, this);
        }
        missionID = SceneManager.GetSceneByName(missionName).buildIndex;
    }

    public void Complete()
    {
        this.isCompleted = true;
    }

    public void Complete(float score)
    {
        this.isCompleted = true;
        this.missionScore = score;
    }

    public void Reset()
    {
        this.isLocked = true;
        this.isCompleted = false;
        this.missionScore = 0f;
    }

    public void Unlock()
    {
        this.isLocked = false;
    }
}
