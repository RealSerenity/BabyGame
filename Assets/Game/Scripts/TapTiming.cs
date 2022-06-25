using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapTiming : MonoBehaviour
{
    public Slider slider;
    private float sliderValue;
    public float sliderSpeed = 1f;
    private bool addValue = true;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (addValue)
        {
            slider.value += Time.deltaTime * sliderSpeed;
        }
        else
        {
            slider.value -= Time.deltaTime * sliderSpeed;
        }

        if (slider.value == slider.maxValue)
        {
            addValue = false;
        }else if (slider.value == slider.minValue)
        {
            addValue = true;
        }
    }
}
