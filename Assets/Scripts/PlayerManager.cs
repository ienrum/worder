using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    const int MAX_WORD_SIZE = 25;
    public string answerWord = "";
    private string playerInput = "";

    public Transform typingSound;
    public Transform removeSound;

    public string PlayerInput
    {
        get { return playerInput; }
    }
        
    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isKeyOf(getKeyToString(), "Return"))
        {
            answerWord = playerInput;
            playerInput = "";
            Debug.Log(answerWord);
        }
        else if (isKeyInAlphabet(getKeyToString()))
        {
            if (playerInput.Length < MAX_WORD_SIZE)
            {
                typingSound.GetComponent<AudioSource>().Play();
                playerInput += getKeyToString().ToLower();
            }
        }
        else if (isKeyOf(getKeyToString(), "Backspace"))
        {
            removeSound.GetComponent<AudioSource>().Play();
            playerInput = delLastKey(playerInput).ToLower();
        }
    }
    private string delLastKey(string input)
    {
        string result = "";
        if (input.Length > 0)
        {
            result = input.Substring(0, playerInput.Length - 1);
        }
        return result;
    }

    private bool isKeyInAlphabet(string input)
    {
        if (input.Count() != 1)
        {
            return false;
        }
        foreach(char c in alphabet)
        {
            if(input.Contains(c)) return true;
        }
        return false;
    }

    private bool isKeyOf(string input,string target)
    {
        if(input == target)
        {
            return true;
        }
        return false;
    }

    private string getKeyToString()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                return keyCode.ToString();
            }
        }
        return "";
    }
}
