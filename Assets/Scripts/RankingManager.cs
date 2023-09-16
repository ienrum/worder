using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public Transform inputField;
    public Transform scrollView;
    public Transform playerNameUI;
    public Transform scoreField;
    public Transform contentFieldPos;

    Dictionary<string, string> playerScoreDict;

    // Start is called before the first frame update
    void Start()
    {
        playerScoreDict = PlayerPrefsDictionary.LoadDictionary("playerScores");
    }

    public void onClicked()
    {
        string playerName = inputField.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        string score = scoreField.GetComponent<TextMeshProUGUI>().text;

        playerScoreDict.Add(playerName, score.Substring(8, score.Length - 8));
        PlayerPrefsDictionary.SaveDictionary(playerScoreDict, "playerScores");

        inputField.gameObject.SetActive(false);
        scoreField.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(true);

        playerNameUI.GetComponent<TextMeshProUGUI>().text = playerName;
        MakeScoreFieldList(playerScoreDict, playerNameUI, contentFieldPos);
    }

    private void MakeScoreFieldList (Dictionary<string,string> playerScoreDict,Transform playerNameUI, Transform contentFieldPos)
    {
        foreach(var kvp in playerScoreDict)
        {
            Transform tempPlayerNameUI = playerNameUI;
            tempPlayerNameUI.GetComponent<TextMeshProUGUI>().text = kvp.Key + " : " + kvp.Value;
            Instantiate(tempPlayerNameUI, contentFieldPos);
        }
    }
}



public class PlayerPrefsDictionary
{
    public static void SaveDictionary(Dictionary<string, string> dict, string prefKey)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var kvp in dict)
        {
            sb.Append(kvp.Key).Append(":").Append(kvp.Value).Append(";");
        }
        PlayerPrefs.SetString(prefKey, sb.ToString());
    }

    public static Dictionary<string, string> LoadDictionary(string prefKey)
    {
        var result = new Dictionary<string, string>();
        var savedString = PlayerPrefs.GetString(prefKey, "");
        if (!string.IsNullOrEmpty(savedString))
        {
            foreach (var kvp in savedString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = kvp.Split(':');
                if (parts.Length == 2)
                {
                    string value = parts[1];
                    result[parts[0]] = value;
                    
                }
            }
        }
        return result;
    }
}
