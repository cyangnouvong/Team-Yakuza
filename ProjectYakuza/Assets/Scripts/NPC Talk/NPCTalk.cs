using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCTalk : MonoBehaviour
{
    public Rigidbody rbody;
    public GameObject NPC;
    public GameObject interactTextBox;
    public GameObject interactText;
    public GameObject NPCDialoguePanel;
    public GameObject NPCDialougeText;
    void FixedUpdate()
    {
        float NPCDistance = -1.0f;
        if (NPC != null)
        {
            NPCDistance = Vector3.Distance(rbody.transform.position, NPC.transform.position);
        }

        if (NPCDistance != -1 && NPCDistance < 2.0f)
        {
            interactText.SetActive(true);
            interactTextBox.SetActive(true);
        }
        else
        {
            interactText.SetActive(false);
            interactTextBox.SetActive(false);
        }

        if (interactText.activeSelf && Input.GetKey(KeyCode.E))
        {
            //FadeBlackScript.fade_out = true;
            //ThirdPersonCamera.followPlayer = false;
            //StartCoroutine(teleport());
            StartCoroutine(PlayNPCDialogue());
        }
    }
    IEnumerator PlayNPCDialogue()
    {
        NPCDialoguePanel.SetActive(true);
        NPCDialougeText.SetActive(true);
        yield return new WaitForSeconds(6f);
        //interactText.SetActive(false);
        //interactTextBox.SetActive(false);
    }
}
