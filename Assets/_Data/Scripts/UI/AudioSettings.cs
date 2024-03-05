using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();

        slider.onValueChanged.AddListener(delegate { ChangedVolume(); });

        InitVolume();
    }

    private void InitVolume()
    {
        slider.minValue = 0;
        slider.value = AudioManager.Instance.audioSource.volume;
        slider.maxValue = 1;
    }    

    private void ChangedVolume()
    {
        AudioManager.Instance.audioSource.volume = slider.value;
    }    
}
