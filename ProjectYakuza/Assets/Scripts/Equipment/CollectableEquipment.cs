using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEquipment : MonoBehaviour
{

    public GameObject sword;
    public static bool swordCollected = false;

    public GameObject NPCDialoguePanel;
    public GameObject NPCDialougeText;

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

                Destroy(this.gameObject);
            }
        }
    }
}
