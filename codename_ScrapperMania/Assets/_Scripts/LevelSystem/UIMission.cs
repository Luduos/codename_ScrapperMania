using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMission : MonoBehaviour
{
    public Text missionNameText;
    public Text missionDescription;
    public Image missionImage;
    public GameObject lockImage;

    private Transform missionScoreParent;
    private Text score;

    private void Awake()
    {
        missionScoreParent = transform.Find("Score").transform;
        score = missionScoreParent.GetComponentInChildren<Text>();
    }

    public void SetScore(float seconds)
    {
        missionScoreParent.gameObject.SetActive(true);
        score.text = System.TimeSpan.FromSeconds(seconds).ToString();
    }

    public void DeactivateScore()
    {
        missionScoreParent.gameObject.SetActive(false);
    }
}
