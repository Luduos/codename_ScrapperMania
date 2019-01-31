using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMission : MonoBehaviour
{
    public bool isHighlighted;

    [SerializeField] private Color normalTextColor;
    [SerializeField] private Color highlightedTextColor;
    [SerializeField] private Text missionNameText;
    [SerializeField] private Image lockImage;
    public Image LockImage { get { return lockImage; } }

    private Transform missionScoreParent;
    private Text score;

    private void Awake()
    {
        missionScoreParent = transform.Find("Score").transform;
        score = missionScoreParent.GetComponentInChildren<Text>();
    }

    public void SetMissionName(string missionName)
    {
        missionNameText.text = missionName;
    }

    public void ChangeColor()
    {
        if (isHighlighted)
        {
            lockImage.color = highlightedTextColor;
            missionNameText.color = highlightedTextColor;
            score.color = highlightedTextColor;
        }
        else
        {
            lockImage.color = normalTextColor;
            missionNameText.color = normalTextColor;
            score.color = normalTextColor;
        }
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
