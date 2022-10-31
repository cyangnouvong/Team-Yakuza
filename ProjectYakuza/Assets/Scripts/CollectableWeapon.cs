using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableWeapon : MonoBehaviour
{
    public AudioSource coin;

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            WeaponCollector wc = c.attachedRigidbody.gameObject.GetComponent<WeaponCollector>();
            if (wc != null)
            {
                //wc.ReceiveSword();
                Destroy(this.gameObject);
                coin.enabled = true;
            }
        }
    }
}
