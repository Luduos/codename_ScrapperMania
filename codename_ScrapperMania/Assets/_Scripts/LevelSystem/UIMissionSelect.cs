using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMissionSelect : MonoBehaviour
{
    [SerializeField]
    private MissionController missionController = null;
    [SerializeField]
    private Text missionDescription = null;
    [SerializeField]
    private Image missionImage = null;
    [SerializeField]
    private Button playButton = null;

    [SerializeField]
    private UIMission missionUI = null;
    [SerializeField]
    private MissionPopup missionPopup = null;

    private UIMission lastSelected = null;
    private Transform missionPanel = null;
    private List<UIMission> missionList = new List<UIMission>();
    private List<Mission> missions = new List<Mission>();

    private void Awake()
    {
        missions = missionController.GetMissions();
        missionPanel = transform;
        for (int i = 0; i < missions.Count; i++)
        {
            missionList.Add(missionUI);
        }
        BuildMissionPanel();
    }

    public void OnStartMission(Mission currentMission)
    {
        if (null != currentMission)
            currentMission.Initialize();
    }

    private void BuildMissionPanel()
    {
        for (int i = 0; i < missions.Count; i++)
        {
            Mission mission = missions[i];
            UIMission instance = Instantiate(missionList[i]);
            instance.transform.SetParent(missionPanel);
            instance.GetComponent<Button>().onClick.AddListener(() => OnMissionSelect(instance, mission));

            instance.SetMissionName(mission.missionName);
            if (!mission.isLocked)
            {
                instance.SetScore(mission.missionScore);
                instance.LockImage.gameObject.SetActive(false);
            }
            else
                instance.LockImage.gameObject.SetActive(true);
        }
    }

    private void OnMissionSelect(UIMission instance, Mission mission)
    {
        playButton.onClick.RemoveAllListeners();
        if (null != lastSelected)
        {
            lastSelected.isHighlighted = false;
            lastSelected.ChangeColor();
        }
        instance.isHighlighted = true;
        instance.ChangeColor();
        lastSelected = instance;
        missionDescription.text = mission.missionDescription;
        missionImage.sprite = mission.backgroundSprite;
        if (mission.isLocked)
        {
            missionPopup.gameObject.SetActive(true);
            missionPopup.SetText("<b>" + mission.missionName + " is currently locked.</b>\nComplete the previous mission to unlock it.");
            Debug.Log("Mission locked");
        }
        else
        {
            playButton.onClick.AddListener(() => OnStartMission(mission));
        }
    }
}
