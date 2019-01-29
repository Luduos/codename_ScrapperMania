using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevel : MonoBehaviour
{
    public Level level;
    public Text levelNameText;
    public GameObject lockImage;

    private Transform scoreParent;
    private Text time;

    private void Awake()
    {
        scoreParent = transform.Find("Score").transform;
        time = scoreParent.GetComponentInChildren<Text>();
    }

    public void SetTime(int seconds)
    {
        scoreParent.gameObject.SetActive(true);
        time.text = System.TimeSpan.FromSeconds(seconds).ToString();
    }

    public void deactivateTime()
    {
        scoreParent.gameObject.SetActive(false);
    }
}
