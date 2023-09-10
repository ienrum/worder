using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : MonoBehaviour
{
    public void onClicked()
    {
        SceneManager.LoadScene("Play",LoadSceneMode.Single);
    }

    public void onGameOver()
    {
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }
}
