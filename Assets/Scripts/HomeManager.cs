using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    public static float score = 10000000000f;
    public TextMeshProUGUI ScoreUI;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Timer"))
        {
            score = Mathf.Min(PlayerPrefs.GetFloat("Timer"), HomeManager.score);
            string scoreString = FormatTime(score);
            ScoreUI.text = "Score : " + scoreString;
        }
    }
    string FormatTime(float seconds)
    {
        int intSeconds = (int)seconds;
        int hours = intSeconds / 3600;
        int minutes = (intSeconds % 3600) / 60;
        int remainingSeconds = intSeconds % 60;

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, remainingSeconds);
    }
}
