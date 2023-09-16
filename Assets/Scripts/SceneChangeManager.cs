using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : MonoBehaviour
{
    public static void goPlay()
    {
        SceneManager.LoadScene("Play", LoadSceneMode.Single);
    }

    public static void goHome()
    {
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }

    public static void goRank()
    {
        SceneManager.LoadScene("Rank", LoadSceneMode.Single);
    }
}
