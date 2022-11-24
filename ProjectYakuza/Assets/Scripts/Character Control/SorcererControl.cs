using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SorcererControl : MonoBehaviour
{
    private Animator anim;
    private int prevIndex = -1;

    void Awake()
    {

        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        anim.SetBool("isPointing", false);
        anim.SetBool("isAngry", false);
        anim.SetBool("isCharge", false);

        if (Dialogue.index != prevIndex && Dialogue.index == 1)
        {
            anim.SetBool("isPointing", true);
            prevIndex += 2;
        }
        if (Dialogue.index != prevIndex && Dialogue.index == 3)
        {
            anim.SetBool("isAngry", true);
            prevIndex += 2;
        }
        if (Dialogue.index != prevIndex && Dialogue.index == 6)
        {
            anim.SetBool("isCharge", true);
            prevIndex += 3;
        }
    }
}