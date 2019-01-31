using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPopup : MonoBehaviour
{
    public Text popupText;

    public void SetText(string text)
    {
        popupText.text = text;
    }
}
