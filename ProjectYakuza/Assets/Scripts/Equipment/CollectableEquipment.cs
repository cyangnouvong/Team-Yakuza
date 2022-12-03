using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEquipment : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            CharacterStats cs = c.attachedRigidbody.gameObject.GetComponent<CharacterStats>();
            if (cs != null)
            {
                GameManager.Instance.playerStats.ItemCount++;
                Destroy(this.gameObject);
            }
        }
    }
}
