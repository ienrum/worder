using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using System.Reflection;
using UnityEditor.Search;
using UnityEngine.UI;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private List<string> wordDictionary = new List<string>();
    string shuffledWords = "";

    public List<Transform> boxUiList;
    public Transform hintUI;
    private PlayerManager playerManager;

    public SceneChangeManager sceneChangeManager;

    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        wordDictionary = FiveLetterWordsList.getRandomFiveWordsList();
        List<char> chars;
        chars = stringToCharList(String.Join("", wordDictionary));

        List<char> shuffledCharList = ShuffleList(chars,100);
        shuffledWords = charListToString(shuffledCharList).ToUpper();

        playerManager = FindAnyObjectByType<PlayerManager>();

        hintUI.GetComponent<TextMeshProUGUI>().text = shuffledWords;
    }

    // Update is called once per frame
    void Update()
    {
        updateBoxUIList(playerManager.PlayerInput,boxUiList);
        updateHintUI(playerManager.PlayerInput,shuffledWords,hintUI);
        if (isComplete(playerManager.PlayerInput,wordDictionary))
        {
            float score = Mathf.Min(timer, HomeManager.score);
            PlayerPrefs.SetFloat("Timer", score);

            PlayerPrefs.Save();
            sceneChangeManager.onGameOver();
        }
        timer += Time.deltaTime;
    }
    private void updateBoxUIList(string input,List<Transform> list)
    {
        List<Color> tempColorList = updateBoxColor(input, wordDictionary);
        int n = list.Count();
        for(int i=0;i<n;i++)
        {
            list[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = i < input.Length ? input[i].ToString() : "";
            list[i].GetComponent<Image>().color = tempColorList[i];
        }
    }

    private void updateHintUI(string input,string shuffledWords,Transform hintUI)
    {
        string tempWords = updateHintColor(input, shuffledWords);

        hintUI.GetComponent<TextMeshProUGUI>().text = tempWords;
    }

    private string updateHintColor(string input, string shuffledWords)
    {
        string tempWords = shuffledWords;
        string tempInput = input;

        for (int i = 0; i < tempInput.Length; i++)
        {
            int index = findIndexOfStringByFiltering(tempWords, tempInput[i].ToString(),isColorString);

            if (index != -1 && !isColorString(tempWords, index))
            {
                string colorString = toColorString(tempWords[index].ToString(), "ae445a");

                tempWords = tempWords.Remove(index, 1);
                tempWords = tempWords.Insert(index, colorString);
            }
        }

        return tempWords;
    }

    private List<Color> updateBoxColor(string input, List<string> wordDictionary)
    {
        string tempInput = input;
        List<Color> tempColorList = Enumerable.Repeat(Color.white,25).ToList();
        int cnt = 0;
        foreach (string word in wordDictionary)
        {
            int index = tempInput.IndexOf(word.ToUpper());
            if(index != -1)
            {
                cnt += 1;
                Color color;
                ColorUtility.TryParseHtmlString("#96c291", out color);
                tempColorList = changeToTargetColor(tempColorList, color, index, word.Length+index);
                
            }
        }
        return tempColorList;
    }

    private List<Color> changeToTargetColor (List<Color> colorList,Color target,int from,int to)
    {
        List<Color> tempColors = new List<Color>(colorList);
        for(int i = from; i < to; i++)
        {
            tempColors[i] = target;
        }
        return tempColors;
    }

    private bool isComplete(string input, List<string> wordDictionary)
    {
        foreach(string word in wordDictionary)
        {
            if (!input.Contains(word.ToUpper()))
            {
                return false;
            }
        }
        return true;
    }

    private string SetCharAt(string str, int index, char c)
    {
        if (index < 0 || index >= str.Length)
            return str; // �ε����� ������ ����� ���� ���ڿ� ��ȯ

        char[] chars = str.ToCharArray();
        chars[index] = c;
        return new string(chars);
    }

    private string toColorString( string input,string colorCode)
    {
        return "<color=#"+colorCode+">"+ input + "</color>";  
    }

    private bool isColorString (string input,int index)
    {
        if (index + 2 >= input.Length)
        {
            return false;
        }
        if (input.Substring(index+1,2) == "</")
        {
            return true;
        }
        return false;
    }

    private delegate bool MyDelegate(string a, int b);


    private int findIndexOfStringByFiltering(string text,string searchTerm,MyDelegate filterFunc)
    {
        int index = 0;
        while ((index = text.IndexOf(searchTerm, index)) != -1)
        {
            if (filterFunc(text, index))
            {
                index += searchTerm.Length;
            }
            else if (!filterFunc(text, index))
            {
                return index;
            }
        }

        return -1;
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
