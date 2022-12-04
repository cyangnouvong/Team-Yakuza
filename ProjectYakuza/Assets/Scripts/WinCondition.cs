using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public GameObject SorcererDialoguePanel;
    public GameObject SorcererDialougeText;

    void Update()
    {
        if (GameManager.Instance.playerStats.ItemCount == 6)
        {
            SorcererDialoguePanel.SetActive(true);
            SorcererDialougeText.SetActive(true);
            if (Dialogue.index == 2)
            {
                SorcererDialoguePanel.SetActive(false);
                SorcererDialougeText.SetActive(false);
                FadeBlackScript.fade_out = true;
                ThirdPersonCamera.followPlayer = false;
                StartCoroutine(switchScene());
            }
        }
    }

    IEnumerator switchScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("WinScene");
    }
}
