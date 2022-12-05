using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemyDamageDealer : MonoBehaviour
{
    bool canDealDamage;
    bool hasDealtDamage;
 
    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;

    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = false;
        //audioSource = GetComponent<AudioSource>();
    }
 
    // Update is called once per frame
    void Update()
    {
        if (canDealDamage && !hasDealtDamage)
        {
            RaycastHit hit;
 
            int layerMask = 1 << 6;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out HealthSystem health))
                {
                    Debug.Log("Player Take Damage");
                    //audioSource.PlayOneShot(audioSource.clip, 1F);
                    //AudioSource.PlayClipAtPoint(damageSound, transform.position);
                    health.TakeDamage(weaponDamage);
                    // health.HitVFX(hit.point);
                    hasDealtDamage = true;
                }
            }
        }
    }
    public void StartDealDamage()
    {
        Debug.Log("Enemy Start Deal Damage");
        canDealDamage = true;
        hasDealtDamage = false;
    }
    public void EndDealDamage()
    {
        canDealDamage = false;
    }
 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
