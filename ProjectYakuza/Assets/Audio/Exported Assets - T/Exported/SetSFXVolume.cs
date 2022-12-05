using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetSFXVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public static float levelsfx = 1f;
    private Slider slider;
    public static float newSlideValuesfx = 1f;

    void Start()
    {
        slider = GetComponent<Slider>();
        mixer.SetFloat("SFXVol", levelsfx);
        slider.value = newSlideValuesfx;
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
        levelsfx = Mathf.Log10(sliderValue) * 20;
        newSlideValuesfx = sliderValue;
    }
}
