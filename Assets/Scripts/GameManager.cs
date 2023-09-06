using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<string> wordDictionary = new List<string>() {"power","coffee","function","bug","man"};
    // Start is called before the first frame update
    void Start()
    {

        List<char> chars = new List<char>();
        chars.AddRange(String.Join(' ',wordDictionary));

        Debug.Log(new string(chars.ToArray()));

        string shuffledWordList = new string(Shuffle(chars).ToArray());

        Debug.Log( shuffledWordList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        List<T> result = new List<T>(new T[n]) ;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n);
            Debug.Log(n+" "+k);
            T temp = list[k];
            list[k] = temp;
            result[k] = temp;
        }
        return result;
    }
    private bool IsWord(string word,List<string> wordDictionary)
    { 
        return wordDictionary.Contains(word.ToLower());
    }
}
