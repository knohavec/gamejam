using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the UI slider
    public AudioSource audioSource; // Reference to the audio source
    private const string VolumePrefKey = "Volume";

    void Start()
    {
        // Load the saved volume value if it exists, otherwise use the default volume
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.5f); // Default to 0.5 if no volume is saved
        audioSource.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Add a listener to call the OnVolumeChanged method whenever the slider value changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // Method called when the slider value changes
    void OnVolumeChanged(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat(VolumePrefKey, value);
    }

    // Ensure that the PlayerPrefs data is saved
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}