using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChurchSceneScript : MonoBehaviour
{
    private int prevIndex = 8;

    void FixedUpdate()
    {
        if (Dialogue.index == 9 && Dialogue.index != prevIndex)
        {
            FadeBlackScript.fade_out = true;
            prevIndex++;
            StartCoroutine(switchScene());
        }
    }

    IEnumerator switchScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("CityScene");
    }
}
