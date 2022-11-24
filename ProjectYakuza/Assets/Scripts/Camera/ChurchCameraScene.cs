using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchCameraScene : MonoBehaviour
{
    private int prevIndex = 0;
    private Quaternion majimaFocus;
    private Quaternion sorcererFocus;
    private bool moveCamera = true;

    void Start()
    {
        majimaFocus = transform.rotation;
        sorcererFocus = Quaternion.Euler(10, 50, 0);
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (Dialogue.index < 6)
        {
            if (Dialogue.index != prevIndex && Dialogue.index % 2 == 1)
            {
                transform.rotation = sorcererFocus;
                prevIndex++;
            }
            else if (Dialogue.index != prevIndex && Dialogue.index % 2 == 0)
            {
                transform.rotation = majimaFocus;
                prevIndex++;
            }
        } else
        {
            if (moveCamera)
            {
                transform.position = new Vector3(17, 6, -16);
                transform.rotation = Quaternion.Euler(13, -30, 0);
                moveCamera = false;
            }
        }
    }
}
