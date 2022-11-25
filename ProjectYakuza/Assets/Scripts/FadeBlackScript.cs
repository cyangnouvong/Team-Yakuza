using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlackScript : MonoBehaviour
{

    public GameObject panel;
    private Image image;
    public static bool fade_in_and_out = false;
    public static bool fade_out = false;
    public static bool fade_in = false;
    private IEnumerator coroutine;

    void Start()
    {
        image = panel.GetComponent<Image>();
    }

   void Update()
    {
        if (fade_in_and_out)
        {
            StartRoutineInOut();
        }
        if (fade_out)
        {
            StartRoutineFadeOut();
        }
        if (fade_in)
        {
            StartRoutineFadeIn();
        }
    }

    void StartRoutineInOut()
    {
        coroutine = FadeInAndOut();
        StartCoroutine(coroutine);
        fade_in_and_out = false;
    }

    void StartRoutineFadeOut()
    {
        coroutine = FadeOut();
        StartCoroutine(coroutine);
        fade_out = false;
    }

    void StartRoutineFadeIn()
    {
        coroutine = FadeIn();
        StartCoroutine(coroutine);
        fade_in = false;
    }

    IEnumerator FadeOut()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            image.color = new Color(0, 0, 0, i);
            yield return null;
        }
        image.color = new Color(0, 0, 0, 1);
    }

    IEnumerator FadeIn()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            image.color = new Color(0, 0, 0, i);
            yield return null;
        }
        image.color = new Color(0, 0, 0, 0);
    }

    IEnumerator FadeInAndOut()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            image.color = new Color(0, 0, 0, i);
            yield return null;
        }
        image.color = new Color(0, 0, 0, 1);

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            image.color = new Color(0, 0, 0, i);
            yield return null;
        }
        image.color = new Color(0, 0, 0, 0);
    }
}
