using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMissionSelect : MonoBehaviour
{
    public Mission[] missions;

    [SerializeField]
    private UIMission missionUI;
    [SerializeField]
    private LevelPopup levelPopup;

    private Transform missionPanel;
    private List<UIMission> missionList = new List<UIMission>();

    private void Start()
    {
        missionPanel = transform;
        for (int i = 0; i < missions.Length; i++)
        {
            missionList.Add(missionUI);
        }
        BuildMissionPanel();
    }

    public void OnStartMission(int missionID)
    {
        missions[missionID].Initialize();
    }

    private void BuildMissionPanel()
    {
        for (int i = 0; i < missions.Length; i++)
        {
            Mission mission = missions[i];
            UIMission instance = Instantiate(missionList[i]);
            instance.transform.SetParent(missionPanel);
            instance.GetComponent<Button>().onClick.AddListener(() => OnMissionSelect(mission));

            if (!mission.isLocked)
            {
                instance.SetScore(mission.missionScore);
                instance.lockImage.SetActive(false);
                instance.missionNameText.text = mission.missionName;
                //instance.missionDescription.text = mission.missionDescription;
                //instance.missionImage.sprite = mission.backgroundSprite;
            }
            else
            {
                instance.lockImage.SetActive(true);
                instance.missionNameText.text = mission.ToString();
            }
        }
    }

    private void OnMissionSelect(Mission mission)
    {

    }
}
