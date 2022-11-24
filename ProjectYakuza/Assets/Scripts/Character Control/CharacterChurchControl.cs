using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterChurchControl : MonoBehaviour
{
    private Animator anim;
    private int prevIndex = 0;

    void Awake()
    {

        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        anim.SetBool("isThumbsUp", false);
        anim.SetBool("isStretching", false);

        if (Dialogue.index != prevIndex && Dialogue.index == 2)
        {
            anim.SetBool("isThumbsUp", true);
            anim.Play("Base Layer.Thumbs Up", 0, 0);
            prevIndex += 2;
        }

        if(Dialogue.index != prevIndex && Dialogue.index == 4)
        {
            anim.SetBool("isStretching", true);
            anim.Play("Base Layer.Arm Stretching", 0, 0);
            prevIndex += 2;
        }

        if (Dialogue.index != prevIndex && Dialogue.index == 6)
        {
            anim.SetBool("isLooking", true);
            anim.Play("Base Layer.Look Around", 0, 0);
            prevIndex += 2;
        }
    }
}