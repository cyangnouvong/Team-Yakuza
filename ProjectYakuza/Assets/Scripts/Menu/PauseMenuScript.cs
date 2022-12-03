
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject menu;
    public GameObject controlScreen;
    public GameObject quitScreen;
    public static bool pause = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
            {
                menu.SetActive(true);
                Time.timeScale = 0;
                pause = true;
            }
            else
            {
                menu.SetActive(false);
                controlScreen.SetActive(false);
                quitScreen.SetActive(false);
                Time.timeScale = 1;
                pause = false;
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pause = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pause = true;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
