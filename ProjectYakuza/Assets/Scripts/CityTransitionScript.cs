using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityTransitionScript : MonoBehaviour
{
    public GameObject introDialoguePanel;
    public GameObject introDialougeText;

    // Start is called before the first frame update
    void Start()
    {
        FadeBlackScript.fade_in = true;
        ThirdPersonCamera.followPlayer = true;
        StartCoroutine(PlayIntroDialogue());
    }

    IEnumerator PlayIntroDialogue()
    {
        yield return new WaitForSeconds(6f);
        introDialoguePanel.SetActive(true);
        introDialougeText.SetActive(true);
    }
}
