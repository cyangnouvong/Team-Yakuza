using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterWinControl : MonoBehaviour
{
    private Animator anim;
    private int prevIndex = 0;
    public GameObject winScreen;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        FadeBlackScript.fade_in = true;
    }

    void FixedUpdate()
    {
        if (Dialogue.index != prevIndex && Dialogue.index == 1)
        {
            anim.SetBool("isVictory", true);
            anim.Play("Base Layer.Victory", 0, 0);
            prevIndex += 1;
        }
        if (Dialogue.index != prevIndex && Dialogue.index == 2)
        {
            winScreen.SetActive(true);
            ThirdPersonCamera.followPlayer = false;
        }
    }
}