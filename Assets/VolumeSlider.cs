using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolumeSlider : MonoBehaviour
{

    public TextMeshProUGUI volumeText;

    public void SetNumberText(float volumeSliderValue)
    {
        volumeText.text = volumeSliderValue.ToString();
    }

}
