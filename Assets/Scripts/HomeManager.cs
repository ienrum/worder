using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreUI;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Timer"))
        {
            string scoreString = StringHelper.FormatTime(PlayerPrefs.GetFloat("Timer"));
            ScoreUI.text = "Score : " + scoreString;
        }
    }
    
}
