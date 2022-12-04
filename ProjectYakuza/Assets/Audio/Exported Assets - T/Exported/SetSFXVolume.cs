using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetSFXVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public static float level = 1f;
    private Slider slider;
    public static float newSlideValue = 1f;

    void Start()
    {
        slider = GetComponent<Slider>();
        mixer.SetFloat("SFXVol", level);
        slider.value = newSlideValue;
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
        level = Mathf.Log10(sliderValue) * 20;
        newSlideValue = sliderValue;
    }
}
