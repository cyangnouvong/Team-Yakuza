using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonControl : MonoBehaviour
{

    public GameObject skeleton1;
    public GameObject skeleton2;
    public GameObject skeleton3;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Dialogue.index == 6)
        {
            skeleton1.SetActive(true);
            skeleton2.SetActive(true);
            skeleton3.SetActive(true);
        }
    }
}
