using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text itemText;
    Image healthSlider;

    void Awake()
    {
        itemText = transform.GetChild(1).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        UpdateHealth();
        UpdateItemCount();
    }

    void UpdateHealth()
    {
        // Debug.Log(GameManager.Instance.playerStats.CurrentHealth);
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        Debug.Log(sliderPercent + "%");
        healthSlider.fillAmount = sliderPercent;
    }

    void UpdateItemCount()
    {
        itemText.text = "Collected Equipment : " + GameManager.Instance.playerStats.characterData.itemCount.ToString() + " / 6";
    }
}
