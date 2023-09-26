using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Transform errorMessageUI;
    public Transform toHomeButton;
    public Transform EnterButton;

    Dictionary<string, int> playerScoreDict = new Dictionary<string, int>()
    {
        {"player1",10 },
    };

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("playerScores"))
            playerScoreDict = PlayerPrefsDictionary.LoadDictionary("playerScores");
    }

    public void showRank()
    {
        string playerName = inputField.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        string scoreText = scoreField.GetComponent<TextMeshProUGUI>().text;

        if (playerScoreDict.ContainsKey(playerName))
        {
            errorMessageUI.GetComponent<TextMeshProUGUI>().text = "Name is duplicated";
            StartCoroutine(ErrorMessageCoroutine(errorMessageUI));
        }
        else
        {
            playerScoreDict.Add(playerName, getScoreFromUI(scoreText));

            PlayerPrefsDictionary.SaveDictionary(playerScoreDict, "playerScores");

            MakeScoreFieldList(playerScoreDict, playerNameUI, contentFieldPos);
            updateUIs();
        }
    }

    private void updateUIs()
    {
        inputField.gameObject.SetActive(false);
        scoreField.gameObject.SetActive(false);
        EnterButton.gameObject.SetActive(false);

        scrollView.gameObject.SetActive(true);
        toHomeButton.gameObject.SetActive(true);
    }

    private static int getScoreFromUI(string score)
    {
        return StringHelper.DecodeFormatTime(score.Substring(8, score.Length - 8));
    }

    private IEnumerator ErrorMessageCoroutine(Transform errorMessageUI)
    {
        yield return new WaitForSeconds(3); // 3�� ���
        errorMessageUI.GetComponent<TextMeshProUGUI>().text = "";
    }

    private void MakeScoreFieldList (Dictionary<string,int> playerScoreDict,Transform playerNameUI, Transform contentFieldPos)
    {
        var sortedDictDesc = playerScoreDict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        int i = 0;
        foreach (var kvp in sortedDictDesc)
        {
            i++;
            Transform tempPlayerNameUI = playerNameUI;
            tempPlayerNameUI.GetComponent<TextMeshProUGUI>().text =  "["+i + "]  " + kvp.Key + " : " + StringHelper.FormatTime(kvp.Value);
            Instantiate(tempPlayerNameUI, contentFieldPos);
        }
    }
}



public class PlayerPrefsDictionary
{
    public static void SaveDictionary(Dictionary<string, int> dict, string prefKey)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var kvp in dict)
        {
            sb.Append(kvp.Key).Append(":").Append(kvp.Value).Append(";");
        }
        PlayerPrefs.SetString(prefKey, sb.ToString());
    }

    public static Dictionary<string, int> LoadDictionary(string prefKey)
    {
        var result = new Dictionary<string, int>();
        var savedString = PlayerPrefs.GetString(prefKey, "");
        if (!string.IsNullOrEmpty(savedString))
        {
            foreach (var kvp in savedString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = kvp.Split(':');
                if (parts.Length == 2)
                {
                    string value = parts[1];
                    result[parts[0]] = int.Parse(value);
                    
                }
            }
        }
        return result;
    }
}
