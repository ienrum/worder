using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    private List<string> wordDictionary = new List<string>() {"power","coffee","function","bug","man"};
    string shuffledWords = "";

    public List<Transform> boxUiList;
    public Transform hintUI;
    private PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        List<char> chars;
        chars = stringToCharList(String.Join("", wordDictionary));

        List<char> shuffledCharList = ShuffleList(chars,100);
        shuffledWords = charListToString(shuffledCharList);

        playerManager = FindAnyObjectByType<PlayerManager>();

        hintUI.GetComponent<TextMeshProUGUI>().text = shuffledWords;
    }

    // Update is called once per frame
    void Update()
    {
        updateBoxUIList(playerManager.PlayerInput,boxUiList);
    }
    private void updateBoxUIList(string input,List<Transform> list)
    {
        int n = list.Count();
        for(int i=0;i<n;i++)
        {
            list[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = i < input.Length ? input[i].ToString() : "";
        }
    }

    private bool IsCorrectWord(string word, List<string> wordDictionary)
    {
        return wordDictionary.Contains(word.ToLower());
    }

    private List<T> ShuffleList<T>(List<T> list,int shuffleCount)
    {
        int n = list.Count;
        List<T> result = list.Select(t => t).ToList();
        while (shuffleCount > 1)
        {
            shuffleCount--;

            int idx = shuffleCount % n;
            int k = UnityEngine.Random.Range(0, shuffleCount %n);

            T temp = result[idx];
            result[idx] = result[k];
            result[k] = temp;
        }
        return result;
    }
    private string charListToString(List<char> chars)
    {
        return new string(chars.ToArray());
    }

    private List<char> stringToCharList(string  word)
    {
        List<char> chars = new List<char>();
        chars.AddRange(word);
        return chars;
    }
}
