using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CollectableEquipment : MonoBehaviour
{

    public GameObject sword;
    public static bool swordCollected = false;

    public GameObject NPCDialoguePanel;
    public GameObject NPCDialougeText;

    public AudioClip collectionSound;

    

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            CharacterStats cs = c.attachedRigidbody.gameObject.GetComponent<CharacterStats>();
            if (cs != null)
            {
                GameManager.Instance.playerStats.ItemCount++;

                if (this.gameObject == sword)
                {
                    swordCollected = true;
                    NPCDialoguePanel.SetActive(true);
                    NPCDialougeText.SetActive(true);
                }

                AudioSource.PlayClipAtPoint(collectionSound, transform.position);
                Destroy(this.gameObject);
                
            }
        }
    }
}
