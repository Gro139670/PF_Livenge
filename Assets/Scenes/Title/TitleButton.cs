using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void QuitButoon()
    {
        Application.Quit();
    }
}
