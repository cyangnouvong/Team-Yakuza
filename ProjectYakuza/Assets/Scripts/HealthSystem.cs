using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class HealthSystem : MonoBehaviour
{
    public GameObject loseScreen;
    [SerializeField] float health = 100;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;
 
    Animator animator;

    public AudioClip damageSound;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
 
    public void TakeDamage(float damageAmount)
    {
        // Debug.Log("dAMAGE AMOUNT" + damageAmount);
        if (damageAmount > 0)
        {
            // play damage audio
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
        GameManager.Instance.playerStats.CurrentHealth -= (int)damageAmount;
        health = (float)GameManager.Instance.playerStats.CurrentHealth;
        Debug.Log(GameManager.Instance.playerStats.CurrentHealth);
        animator.SetTrigger("damage");
        // CameraShake.Instance.ShakeCamera(2f, 0.2f);
 
        if (health <= 0)
        {
            Die();
        }
    }
 
    void Die()
    {
        // Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(this.gameObject);
        loseScreen.SetActive(true);
    }
    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
 
    }
}