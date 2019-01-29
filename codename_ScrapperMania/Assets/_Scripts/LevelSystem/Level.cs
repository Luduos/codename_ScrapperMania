using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int ID { get; set; }
    public string LevelName { get; set; }
    public bool Completed { get; set; }
    public int Score { get; set; }
    public bool Locked { get; set; }

    public Level(int id, string levelName, bool completed, int score, bool locked)
    {
        this.ID = id;
        this.LevelName = levelName;
        this.Completed = completed;
        this.Score = score;
        this.Locked = locked;
    }

    public void Complete()
    {
        this.Completed = true;
    }

    public void Complete(int score)
    {
        this.Completed = true;
        this.Score = score;
    }

    public void Lock()
    {
        this.Locked = true;
    }

    public void Unlock()
    {
        this.Locked = false;
    }
}
