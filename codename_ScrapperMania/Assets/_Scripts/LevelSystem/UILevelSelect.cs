using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSelect : MonoBehaviour
{
    [SerializeField]
    private UILevel levelUI;
    [SerializeField]
    private LevelPopup levelPopup;

    private Transform levelSelectPanel;
    private int currentPage;
    private List<UILevel> levelList = new List<UILevel>();

    void Start()
    {
        levelSelectPanel = transform;

        for(int i = 0; i < LevelController.instance.levels.Count; i++)
        {
            levelList.Add(levelUI);
        }
        BuildLevelPage(0);
    }

  
    private void BuildLevelPage(int page)
    {
        currentPage = page;
        int pageSize = 11;
        List<UILevel> pageList = levelList.Skip(page * pageSize).Take(pageSize).ToList();

        for (int i = 0; i < pageList.Count; i++)
        {
            Level level = LevelController.instance.levels[(page * pageSize) + i];
            UILevel instance = Instantiate(pageList[i]);
            instance.transform.SetParent(levelSelectPanel);
            instance.GetComponent<Button>().onClick.AddListener(() => SelectLevel(level));

            if(!level.Locked)
            {
                instance.SetTime(level.Score);
                instance.lockImage.SetActive(false);
                instance.levelNameText.text = level.LevelName;
            }
            else
            {
                instance.lockImage.SetActive(true);
                instance.levelNameText.text = "Mission " + level.ID;
            }
        }
    }

    /*
    private void RemoveItemsFromPage()
    {
        for(int i = 0; i < levelSelectPanel.childCount; i++)
        {
            Destroy(levelSelectPanel.GetChild(i).gameObject;
        }
    }
    */

    /*
    public void NextPage()
    {
        BuildLevelPage(currentPage + 1);
    }

    public void PreviousPage()
    {
        BuildLevelPage(currentPage - 1);
    }
    */

    private void SelectLevel(Level level)
    {
        if (level.Locked)
        {
            levelPopup.gameObject.SetActive(true);
            levelPopup.SetText("<b>Mission " + level.ID + " is currently locked.</b>\nComplete Mission " + (level.ID - 1) + " to unlock it.");
            Debug.Log("Level locked");
        }
        else
        {
            Debug.Log("Go to Level: " + level.ID);
            LevelController.instance.StartLevel(level.LevelName);
        }
    }
}
