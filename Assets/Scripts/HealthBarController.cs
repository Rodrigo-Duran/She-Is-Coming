using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{

    #region Variables

    // Components
    private Slider slider;

    #endregion

    #region MainMethods
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    #endregion

    #region HealthBarHandler

    // Method to update enemy health bar
    public void UpdateHealthBarValue(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    #endregion
}
