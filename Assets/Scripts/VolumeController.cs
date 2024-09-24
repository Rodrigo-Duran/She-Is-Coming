using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{

    #region Variables

    // Global variables
    public static float volume = 1;

    // Volume 
    [SerializeField] private Slider volumeSlider;
    private TextMeshProUGUI volumeLabel;

    #endregion

    #region MainMethods

    private void Start()
    {
        volumeLabel = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        volume = volumeSlider.value;
        float volumeFloat = volumeSlider.value * 100;
        int volumeInt = ((int)volumeFloat);
        volumeLabel.text = volumeInt.ToString();
    }

    #endregion

}
