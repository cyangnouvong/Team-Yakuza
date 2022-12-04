using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    public int MaxHealth
    {
        get{if(characterData != null){return characterData.maxHealth;} else{return 100;}}
        set{characterData.maxHealth = value;}
    }
    public int CurrentHealth
    {
        get { if (characterData != null) { return characterData.currentHealth; } else { return 100; } }
        set { characterData.currentHealth = value; }
    }

    public int ItemCount
    {
        get { if (characterData != null) { return characterData.itemCount; } else { return 0; } }
        set { characterData.itemCount = value; }
    }
    void Start()
    {
        characterData.itemCount = 0;
        characterData.currentHealth = 100;
    }
}
