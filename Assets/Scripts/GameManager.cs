using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using System.Reflection;
using UnityEngine.UI;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.SocialPlatforms.Impl;
using System.Threading;

public class GameManager : MonoBehaviour
{
    string greenHexString = "";
    string yellowHexString = "";
    string redHexString = "";
    string backHexString = "";


    private List<string> wordDictionary = new List<string>();
    string shuffledWords = "";

    public List<Transform> boxUiList;
    public Transform hintUI;
    public Transform ScoreUI;
    public Transform TitleUI;
    public Transform hintTitle;

    public Transform failedSound;
    public Transform successSound;

    public Camera camera;

    private PlayerManager playerManager;

    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        List<string> colorHexStrings = ColorHelper.getRandomColorList();

        greenHexString = colorHexStrings[0];
        yellowHexString = colorHexStrings[1];
        redHexString = colorHexStrings[2];
        backHexString = colorHexStrings[3];

        camera.GetComponent<Camera>().backgroundColor = ColorHelper.HexToColor(backHexString);

        ScoreUI.GetComponent<TextMeshProUGUI>().text = ColorHelper.toColorString(ScoreUI.GetComponent<TextMeshProUGUI>().text, greenHexString);
        TitleUI.GetComponent<TextMeshProUGUI>().text = ColorHelper.toColorString(TitleUI.GetComponent<TextMeshProUGUI>().text, greenHexString);
        hintUI.GetComponent<TextMeshProUGUI>().text = ColorHelper.toColorString(shuffledWords, redHexString);

        List<string> targetWords = StringHelper.getRandomFiveWordsList();
        wordDictionary = targetWords.GetRange(0, targetWords.Count-1);
        hintTitle.GetComponent<TextMeshProUGUI>().text = ColorHelper.toColorString(targetWords[targetWords.Count - 1], ColorHelper.ColorToHex(ColorHelper.LerpHexColor(greenHexString,redHexString,0.5f)));

        foreach(string word in wordDictionary)
        {
            Debug.Log(word);
        }
        shuffledWords = StringHelper.GetShuffledLetters(wordDictionary);

        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isComplete(playerManager.PlayerInput, wordDictionary))
        {
            QuitGame(timer);
        }
        updateSound(failedSound, successSound, boxUiList, playerManager.PlayerInput);
        boxUiList = updateBoxUIList(playerManager.PlayerInput, boxUiList);
        updateHintUI(playerManager.PlayerInput, shuffledWords, hintUI);
        timer += Time.deltaTime;
        ScoreUI.GetComponent<TextMeshProUGUI>().text = updateScoreText(timer, greenHexString);
    }
    private string updateScoreText(float timer, string greenHexString)
    {
        return ColorHelper.toColorString(StringHelper.FormatTime(timer), greenHexString);
    }
    private void QuitGame(float timer)
    {
        PlayerPrefs.SetFloat("Timer", timer);
        PlayerPrefs.Save();

        SceneChangeManager.goRank();
    }
    private List<Transform> updateBoxUIList(string playerInput,List<Transform> list)
    {
        List<Transform> tempList = new List<Transform>(list);

        tempList = updateBoxUIListText(tempList, playerInput);

        List<Color> colorList = makeColorList(playerInput, wordDictionary);
        tempList = updateBoxUIListColor(tempList, colorList);

        return tempList;
    }
    private void updateHintUI(string input, string shuffledWords, Transform hintUI)
    {
        string tempWords = updateHintColor(input, ColorHelper.toColorString(shuffledWords, redHexString));

        hintUI.GetComponent<TextMeshProUGUI>().text = tempWords;
    }
    private bool isComplete(string input, List<string> wordDictionary)
    {
        foreach (string word in wordDictionary)
        {
            if (!input.Contains(word))
            {
                return false;
            }
        }

        return true;
    }
    private List<Transform> updateBoxUIListText(List<Transform> boxUIListTransform, string playerInput)
    {
        List<Transform> tempList = new List<Transform>(boxUIListTransform);
        tempList = updateListByValue(tempList, playerInput, (tempList, playerInput, i) => tempList[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = i < playerInput.Length ? ColorHelper.toColorString(playerInput[i].ToString(), ColorHelper.ColorToHex(ColorHelper.LerpHexColor(yellowHexString,redHexString,0.5f))) : "");
        return tempList;
    }
    private List<Transform> updateBoxUIListColor(List<Transform> boxUIListTransform, List<Color> colorList)
    {
        List<Transform> tempList = new List<Transform>(boxUIListTransform);
        tempList = updateListByValue(tempList, colorList, (tempList, colorList, i) => tempList[i].GetComponent<Image>().color = colorList[i]);
        tempList = updateListByValue(tempList, colorList, (tempList, colorList, i) => tempList[i].GetComponent<Outline>().effectColor = ColorHelper.LerpHexColor(greenHexString,yellowHexString,0.5f));
        return tempList;
    }
    private delegate void UpdateTarget<T, V>(T target, V value);
    private delegate void UpdateTargetList<T, V>(List<T> target, V value, int index);
    private List<T> updateListByValue<T, V>(List<T> list, V value, UpdateTargetList<T, V> updateTargetList)
    {
        List<T> tempList = new List<T>(list);
        int n = list.Count();
        for (int i = 0; i < n; i++)
        {
            updateTargetList(tempList, value, i);
        }
        return tempList;
    }
    private List<Color> makeColorList(string playerInput, List<string> correctWords)
    {
        string tempPlayerInput = playerInput;
        List<Color> colorList = Enumerable.Repeat(ColorHelper.HexToColor(backHexString), 25).ToList();
        colorList = makeColorListByCorrectWords(correctWords, tempPlayerInput, colorList);
        return colorList;
    }

    private List<Color> makeColorListByCorrectWords(List<string> correctWords, string tempPlayerInput, List<Color> colorList)
    {
        List<Color> tempColorList = new List<Color>(colorList);
        foreach (string correctWord in correctWords)
        {
            tempColorList = makeColorListByCorrectWord(tempPlayerInput, tempColorList, correctWord);
        }

        return tempColorList;
    }

    private List<Color> makeColorListByCorrectWord(string tempPlayerInput, List<Color> colorList, string correctWord)
    {
        Color green = ColorHelper.HexToColor(greenHexString);
        Color yellow = ColorHelper.HexToColor(yellowHexString);
        Color red = ColorHelper.HexToColor(redHexString);

        List<Color> tempColorList = new List<Color> (colorList);
        List<int> firstLetterIndicesOfWord = GetCorrectFirstLetterIndicesOfPlayerInput(tempPlayerInput, correctWord);

        foreach (int wordFirstIndex in firstLetterIndicesOfWord)
        {
            if (isNotFoundIndex(wordFirstIndex)) { continue; }
            if (isAbleToChangeColor(tempPlayerInput, tempColorList, correctWord, green, wordFirstIndex))
            {
                int overlapLength = letterOverlapLength(correctWord, tempPlayerInput, wordFirstIndex);

                Color color;
                color = makeColorByOverlapLength(yellowHexString, greenHexString, correctWord, overlapLength);
                tempColorList = ColorHelper.makeColorListByTargetColor(tempColorList, color, wordFirstIndex, wordFirstIndex + overlapLength);
            }
            if (isOverlapPrevWord(tempPlayerInput, correctWord, wordFirstIndex))
            {
                tempColorList = ColorHelper.makeColorListByTargetColor(tempColorList, red, wordFirstIndex, wordFirstIndex + correctWord.Length);
            }
        }

        return tempColorList;
    }

    private bool isOverlapPrevWord(string tempPlayerInput, string correctWord, int wordFirstIndex)
    {
        if (tempPlayerInput.Length -  wordFirstIndex < correctWord.Length)
        {
            return false;
        }
        return tempPlayerInput.Substring(wordFirstIndex, correctWord.Length) == correctWord && tempPlayerInput.IndexOf(correctWord) != wordFirstIndex;
    }
    private bool updateSound(Transform failedSound,Transform successSound, List<Transform> boxUIListTransform,string playerInput)
    {
        int n = playerInput.Length - 1;
        if (boxUIListTransform == null || n <0)
        {
            Debug.Log("ok2");
            return false;
        }
        else
        {
            if (isCorrectForCompeteWord(boxUIListTransform[n]))
            {
                Debug.Log("ok");
                successSound.GetComponent<AudioSource>().Play();
            }else if (isNotCorrectForCompeteWord(boxUIListTransform[n]))
            {
                failedSound.GetComponent<AudioSource>().Play();
            }
        }
        return true;
    }
    private bool isCorrectForCompeteWord(Transform boxUITransform) {
        Debug.Log(boxUITransform.GetComponent<Image>().color);
        if (boxUITransform.GetComponent<Image>().color == ColorHelper.HexToColor(greenHexString))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private bool isNotCorrectForCompeteWord(Transform boxUITransform)
    {
        if (boxUITransform.GetComponent<Image>().color == ColorHelper.HexToColor(redHexString))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private List<int> GetCorrectFirstLetterIndicesOfPlayerInput(string playerInput, string correctWord)
    {
        string firstLetterOfWord = correctWord[0].ToString();
        return StringHelper.GetAllIndicesOf(playerInput, firstLetterOfWord);
    }


    private bool isAbleToChangeColor(string tempPlayerInput, List<Color> colorList, string word, Color green, int wordFirstIndex)
    {
        return ColorHelper.isDifferentColor(colorList[wordFirstIndex], green) && isStringOverlapCountUpperThan1(word, tempPlayerInput, wordFirstIndex);
    }

    private bool isNotFoundIndex(int index)
    {
        return index == -1;
    }

    private bool isStringOverlapCountUpperThan1(string word1,string word2,int idx)
    {
        return letterOverlapLength(word1,word2,idx) > 1;
    }
    private Color makeColorByOverlapLength(string yellowHexString, string greenHexString, string word, int overlapCount)
    {
        Color yellowToGreen = ColorHelper.LerpHexColor(yellowHexString, greenHexString, overlapCount / (word.Length + 0.0f));
        return yellowToGreen;
    }

    private string updateHintColor(string input, string shuffledWords)
    {
        string tempWords = shuffledWords;
        string tempInput = input;
        int i = 0;

        while (tempInput.Length > i)
        {
            int index = findIndexOfStringByFiltering(tempWords, tempInput[i].ToString(), isAbleToColored);
            if (index != -1 && isAbleToColored(tempWords, index, greenHexString))
            {
                string colorString = ColorHelper.toColorString(tempWords[index].ToString(), greenHexString);

                tempWords = tempWords.Remove(index, 1);
                tempWords = tempWords.Insert(index, colorString);
            }
            i++;
        }

        return tempWords;
    }

    private bool isAbleToColored(string tempWords, int index,string hexString)
    {
        return !ColorHelper.isColorString(tempWords, index, hexString) && !ColorHelper.isColorHexString(tempWords, index);
    }

    private int letterOverlapLength(string word,string input,int startIndex)
    {
        int cnt = 0;
        int n = Mathf.Min(word.Length, input.Length - startIndex);
        for(int i = 0; i < n; i++)
        {
            int idx = i + startIndex;
            if (input[idx] == word[i])
            {
                cnt += 1;
            }
            else
            {
                return cnt;
            }
        }
        return cnt;
    }

    private delegate bool MyDelegate(string a, int b,string baseHex);
    private int findIndexOfStringByFiltering(string text,string searchTerm,MyDelegate filterFunc)
    {
        int index = 0;
        while ((index = text.IndexOf(searchTerm, index)) != -1)
        {
            if (!filterFunc(text, index,greenHexString))
            {
                index += searchTerm.Length;
            }
            else if (filterFunc(text, index, greenHexString))
            {
                return index;
            }
        }

        return -1;
    }
}

